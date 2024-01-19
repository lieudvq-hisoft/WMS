using AutoMapper;
using Confluent.Kafka;
using Data.Common.PaginationModel;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Data.Models;
using Data.Utils.Paging;
using Microsoft.EntityFrameworkCore;
using Services.Utils;

namespace Services.Core;

public interface IPickingRequestService
{
    Task<ResultModel> Create(PickingRequestCreateModel model);
    Task<ResultModel> Update(PickingRequestUpdateModel model);
    Task<ResultModel> Get(PagingParam<PickingRequestSortCriteria> paginationModel, PickingRequestSearchModel model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> Complete(PickingRequestCompleteModel model);
    Task<ResultModel> GetWeeklyReport();
    Task<ResultModel> GetDetail(Guid id);
    Task<ResultModel> GetCompleted(PagingParam<PickingRequestSortCriteria> paginationModel, PickingRequestCompleteSearchModel model);
    Task<ResultModel> AssignUser(PickingRequestUserCreateModel model);
    Task<ResultModel> UnAssignUser(UnAssignModel model);

}
public class PickingRequestService : IPickingRequestService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IProducer<Null, string> _producer;

    public PickingRequestService(AppDbContext dbContext, IMapper mapper, IProducer<Null, string> producer)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _producer = producer;
    }

    public async Task<ResultModel> Complete(PickingRequestCompleteModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                var pickingRequest = _dbContext.PickingRequest
                    .Include(_ => _.Product).ThenInclude(_ => _.Inventories)
                    .Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
                if (pickingRequest == null)
                {
                    result.ErrorMessage =  "Picking request not exists";
                    result.Succeed = false;
                    return result;
                }
                if (pickingRequest.Status == PickingRequestStatus.Completed)
                {
                    result.ErrorMessage = "Picking request is completed";
                    result.Succeed = false;
                    return result;
                }

                var inventories = pickingRequest.Product.Inventories.Where(_ => _.IsAvailable && !_.IsDeleted);
                if (inventories.Count() < pickingRequest.Quantity)
                {
                    result.ErrorMessage = "Out of stock";
                    result.Succeed = false;
                    return result;
                }
                var list = inventories.Where(_ => model.ListInventoryId.Contains(_.Id));
                if (list.Count() != pickingRequest.Quantity)
                {
                    result.ErrorMessage = "In the inventory list, there is inventory that does not exist corresponding to the product";
                    result.Succeed = false;
                    return result;
                }
                foreach (var inventory in list)
                {
                    var pickingRequestInventoryAdd = new Data.Entities.PickingRequestInventory
                    {
                        InventoryId = inventory.Id,
                        PickingRequestId = pickingRequest.Id,
                    };
                    _dbContext.PickingRequestInventory.Add(pickingRequestInventoryAdd);

                    inventory.IsAvailable = false;
                    inventory.DateUpdated = DateTime.Now;
                    _dbContext.Inventory.Update(inventory);
                }
                pickingRequest.Status = PickingRequestStatus.Completed;
                pickingRequest.DateUpdated = DateTime.Now;
                _dbContext.PickingRequest.Update(pickingRequest);
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                result.Succeed = true;
                result.Data = pickingRequest.Id;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                await transaction.RollbackAsync();
            }
        }
        return result;
    }

    public async Task<ResultModel> AssignUser(PickingRequestUserCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var pickingRequest = _dbContext.PickingRequest
                .Include(_ => _.PickingRequestUsers)
                .Include(_ => _.Product)
                .Where(_ => _.Id == model.PickingRequestId && !_.IsDeleted).FirstOrDefault();
            var user = _dbContext.User.Where(_ => _.Id == model.ReceivedBy && !_.IsDeleted).FirstOrDefault();
            if (user == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            if (pickingRequest == null)
            {
                result.ErrorMessage = "Picking request not exists";
                result.Succeed = false;
                return result;
            }
            if (pickingRequest.Status == PickingRequestStatus.Completed)
            {
                result.ErrorMessage = "Picking request is completed";
                result.Succeed = false;
                return result;
            }
            if(pickingRequest.PickingRequestUsers.Count() != 0)
            {
                result.ErrorMessage = "Picking request has been assigned to another user";
                result.Succeed = false;
                return result;
            }
            var pickingRequestUser = new PickingRequestUser { PickingRequestId = pickingRequest.Id, ReceivedBy = user.Id };
            _dbContext.PickingRequestUser.Add(pickingRequestUser);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = pickingRequestUser.Id;

            var kafkaModel = new KafkaModel { UserReceiveNotice = new List<Guid>() { user.Id}, Payload = _mapper.Map<PickingRequest, PickingRequestModel>(pickingRequest!) };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(kafkaModel);
            await _producer.ProduceAsync("pickingrequest-assign-user", new Message<Null, string> { Value = json });
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UnAssignUser(UnAssignModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var pickingRequest = _dbContext.PickingRequest.Where(_ => _.Id == model.PickingRequestId && !_.IsDeleted).FirstOrDefault();
            if (pickingRequest == null)
            {
                result.ErrorMessage = "Picking request not exists";
                result.Succeed = false;
                return result;
            }
            if (pickingRequest.Status == PickingRequestStatus.Completed)
            {
                result.ErrorMessage = "Picking request is completed";
                result.Succeed = false;
                return result;
            }
            var pickingRequestUser = _dbContext.PickingRequestUser.Where(_ => _.Id == model.PickingRequestUserId).FirstOrDefault();
            if (pickingRequestUser == null)
            {
                result.ErrorMessage = "Picking request user not exists with id: " + model.PickingRequestUserId;
                result.Succeed = false;
                return result;
            }
            _dbContext.PickingRequestUser.Remove(pickingRequestUser);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = pickingRequest.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Create(PickingRequestCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var order = _dbContext.Order.Where(_ => _.Id == model.OrderId && !_.IsDeleted).FirstOrDefault();
            if (order == null)
            {
                result.ErrorMessage = "Order not exists";
                result.Succeed = false;
                return result;
            }
            var product = _dbContext.Product.Include(_ => _.Inventories).Where(_ => _.Id == model.ProductId && !_.IsDeleted).FirstOrDefault();
            if(product == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            //check inventory
            var inventories = product.Inventories.Where(_ => _.IsAvailable && !_.IsDeleted).ToList();
            if(inventories.Count() < model.Quantity)
            {
                result.ErrorMessage = "Out of stock";
                result.Succeed = false;
                return result;
            }
            //end check inventory

            var data = _mapper.Map<PickingRequestCreateModel, PickingRequest>(model);
            _dbContext.PickingRequest.Add(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = data.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Delete(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.PickingRequest.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Picking request not exists!";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.PickingRequest.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = data.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
    public async Task<ResultModel> GetDetail(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.PickingRequest
                .Include(_ => _.Order).ThenInclude(_ => _.SentByUser)
                .Include(_ => _.Product)
                .Include(_ => _.PickingRequestInventories).ThenInclude(_ => _.Inventory).ThenInclude(_ => _.InventoryLocations).ThenInclude(_ => _.Location)
                .Include(_ => _.PickingRequestUsers).ThenInclude(_ => _.ReceivedByUser).Where(_ => _.Id == id).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "PickingRequest not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = _mapper.Map<PickingRequestDetailModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Get(PagingParam<PickingRequestSortCriteria> paginationModel, PickingRequestSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.PickingRequest
                .Include(_ => _.Product).ThenInclude(_ => _.Inventories)
                .Include(_ => _.PickingRequestUsers).ThenInclude(_ => _.ReceivedByUser)
                .Include(_ => _.Order).ThenInclude(_ => _.SentByUser)
                .Where(delegate (PickingRequest p)
            {
                if (
                    (MyFunction.ConvertToUnSign(p.Note ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (p.PickingRequestUsers.FirstOrDefault()!.ReceivedByUser!.UserName!.ToUpper().Contains(model.SearchValue ?? "".ToUpper())
                    || (p.PickingRequestUsers.FirstOrDefault()!.ReceivedByUser.Email!.ToUpper().Contains(Uri.UnescapeDataString(model.SearchValue ?? "").ToUpper())
                    )))
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var pickingRequests = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            pickingRequests = pickingRequests.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<PickingRequestModel>(pickingRequests); 
            paging.Data = viewModels;
            result.Data = paging;
            result.Succeed = true;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(PickingRequestUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.PickingRequest.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Picking request not exists";
                result.Succeed = false;
                return result;
            }
            if (data.Status == PickingRequestStatus.Completed)
            {
                result.ErrorMessage = "Receipt is complete, cannot be updated";
                result.Succeed = false;
                return result;
            }
            if (model.Note != null)
            {
                data!.Note = model.Note;
            }
            if (model.Quantity != null)
            {
                data!.Quantity = (int)model.Quantity;
            }
            data!.DateUpdated = DateTime.Now;
            _dbContext.PickingRequest.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<PickingRequest, PickingRequestModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetWeeklyReport()
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var days = MyFunction.Get7DaysWithToday();
            days.Reverse();
            var reports = new List<DailyReportPickingRequest>();
            foreach (var day in days)
            {
                var pickingRequestCompleted = _dbContext.PickingRequest
                    .Where(_ =>
                        new DateTime(_.DateCreated.Year, _.DateCreated.Month, _.DateCreated.Day) == day
                        && _.Status == PickingRequestStatus.Completed
                        && !_.IsDeleted).ToList();
                var pickingRequestPending = _dbContext.PickingRequest
                    .Where(_ =>
                        new DateTime(_.DateCreated.Year, _.DateCreated.Month, _.DateCreated.Day) == day
                        && _.Status == PickingRequestStatus.Pending
                        && !_.IsDeleted).ToList();
                var report = new DailyReportPickingRequest()
                {
                    Date = day,
                    TotalCompleted = pickingRequestCompleted.Count(),
                    TotalQuantityCompleted = pickingRequestCompleted.Sum(_ => _.Quantity),
                    TotalPending = pickingRequestPending.Count(),
                    TotalQuantityPending = pickingRequestPending.Sum(_ => _.Quantity)
                };
                reports.Add(report);
            }
            result.Succeed = true;
            result.Data = reports;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetCompleted(PagingParam<PickingRequestSortCriteria> paginationModel, PickingRequestCompleteSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.PickingRequest
                .Include(_ => _.Order).ThenInclude(_ => _.SentByUser)
                .Include(_ => _.Product).ThenInclude(_ => _.Inventories)
                .Include(_ => _.PickingRequestUsers).ThenInclude(_ => _.ReceivedByUser).Where(delegate (PickingRequest p)
            {
                if (
                    (MyFunction.ConvertToUnSign(p.Note ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    ||
                    (MyFunction.ConvertToUnSign(p.Product.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (p.PickingRequestUsers.FirstOrDefault()!.ReceivedByUser!.UserName!.ToUpper().Contains(model.SearchValue ?? "".ToUpper())
                    || (p.PickingRequestUsers.FirstOrDefault()!.ReceivedByUser!.Email!.ToUpper().Contains(Uri.UnescapeDataString(model.SearchValue ?? "").ToUpper())
                    || (p.Product.SerialNumber.Trim().ToUpper().Contains(Uri.UnescapeDataString(model.SearchValue ?? "").ToUpper())
                    ))))
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => _.Status == PickingRequestStatus.Completed && !_.IsDeleted);
            if(model.DateCompleted != null)
            {
                data = data.Where(_ => 
                _.DateUpdated.Year == model.DateCompleted.Value.Year &&
                _.DateCreated.Month == model.DateCompleted.Value.Month &&
                _.DateUpdated.Day == model.DateCompleted.Value.Day);
            }
            if (model.DateCreated != null)
            {
                data = data.Where(_ =>
                _.DateCreated.Year == model.DateCreated.Value.Year &&
                _.DateCreated.Month == model.DateCreated.Value.Month &&
                _.DateCreated.Day == model.DateCreated.Value.Day);
            }
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var pickingRequests = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            pickingRequests = pickingRequests.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<PickingRequestModel>(pickingRequests);
            paging.Data = viewModels;
            result.Data = paging;
            result.Succeed = true;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
