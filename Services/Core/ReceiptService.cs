using System;
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

public interface IReceiptService
{
    Task<ResultModel> Create(ReceiptCreateModel model);
    Task<ResultModel> Update(ReceiptUpdateModel model);
    Task<ResultModel> Get(PagingParam<ReceiptSortCriteria> paginationModel, ReceiptSearchModel model);
    Task<ResultModel> GetReceiptPending(PagingParam<ReceiptSortCriteria> paginationModel, ReceiptSearchModel model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> Complete(ReceiptCompleteModel model);
    Task<ResultModel> GetWeeklyReport();
    Task<ResultModel> GetDetail(Guid id);

}
public class ReceiptService : IReceiptService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IProducer<Null, string> _producer;

    public ReceiptService(AppDbContext dbContext, IMapper mapper, IProducer<Null, string> producer)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _producer = producer;
    }

    public async Task<ResultModel> Complete(ReceiptCompleteModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var receipt = _dbContext.Receipt
                .Include(_ => _.Product).ThenInclude(_ => _.Inventories)
                .Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (receipt == null)
            {
                result.ErrorMessage = "Receipt not exists";
                result.Succeed = false;
                return result;
            }

            if (receipt.Status == ReceiptStatus.Completed)
            {
                result.ErrorMessage = "Receipt is completed";
                result.Succeed = false;
                return result;
            }

            var location = _dbContext.Location.Where(_ => _.Id == model.LocationId).FirstOrDefault();
            if (location == null)
            {
                result.ErrorMessage = "Location not exists";
                result.Succeed = false;
                return result;
            }

            for (int i = 0; i < receipt.Quantity; i++)
            {
                var inventory = new Inventory
                {
                    ProductId = receipt.ProductId,
                    QuantityOnHand = 1,
                    SerialCode = receipt.Product.SerialNumber + DateTime.Now.ToBinary(),
                };

                _dbContext.Add(inventory);

                var inventoryLocation = new InventoryLocation
                {
                    InventoryId = inventory.Id,
                    LocationId = location.Id,
                };

                _dbContext.Add(inventoryLocation);
            }
            receipt.Status = ReceiptStatus.Completed;
            receipt.DateUpdated = DateTime.Now;
            _dbContext.Receipt.Update(receipt);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            result.Succeed = true;
            result.Data = receipt.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            await transaction.RollbackAsync();
        }
        return result;
    }

    public async Task<ResultModel> Create(ReceiptCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var supplier = _dbContext.Supplier.Where(_ => _.Id == model.SupplierId && !_.IsDeleted).FirstOrDefault();
            if (supplier == null)
            {
                result.ErrorMessage = "Supplier not exists";
                result.Succeed = false;
                return result;
            }

            var receivedBy = _dbContext.User.Where(_ => _.Id == model.ReceivedBy && !_.IsDeleted).FirstOrDefault();
            if (receivedBy == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            var data = _mapper.Map<ReceiptCreateModel, Receipt>(model);
            _dbContext.Receipt.Add(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = data.Id;

            var receipt = _dbContext.Receipt.Include(_ => _.Product).Where(_ => _.Id == data.Id).FirstOrDefault();
            var userReceiveNotice = _dbContext.User.Include(_ => _.UserRoles).ThenInclude(_ => _.Role)
                .Where(_ => _.UserRoles.Any(ur => ur.Role.NormalizedName == "ADMIN") && _.IsActive && !_.IsDeleted)
                .Select(_ => _.Id).ToList();

            var kafkaModel = new KafkaModel { UserReceiveNotice = userReceiveNotice, Payload = _mapper.Map<Receipt, ReceiptModel>(receipt!) };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(kafkaModel);
            await _producer.ProduceAsync("receipt-create-new", new Message<Null, string> { Value = json });

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
            var data = _dbContext.Receipt.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Receipt not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.Receipt.Update(data);
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

    public async Task<ResultModel> Get(PagingParam<ReceiptSortCriteria> paginationModel, ReceiptSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Receipt.Include(_ => _.Product).Include(_ => _.ReceivedByUser).Include(_ => _.Supplier).Where(delegate (Receipt r)
            {
                if (
                    (MyFunction.ConvertToUnSign(r.Supplier.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    ||
                    (MyFunction.ConvertToUnSign(r.Product.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (r.Supplier.Phone.ToUpper().Contains(model.SearchValue ?? "".ToUpper())
                    || (r.ReceivedByUser.Email.ToUpper().Contains(Uri.UnescapeDataString(model.SearchValue ?? "").ToUpper())
                    )))
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var receipts = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            receipts = receipts.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ReceiptModel>(receipts);
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

    public async Task<ResultModel> GetReceiptPending(PagingParam<ReceiptSortCriteria> paginationModel, ReceiptSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Receipt.Include(_ => _.Product).Include(_ => _.ReceivedByUser).Include(_ => _.Supplier).Where(delegate (Receipt r)
            {
                if (
                    (MyFunction.ConvertToUnSign(r.Supplier.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    ||
                    (MyFunction.ConvertToUnSign(r.Product.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (r.Supplier.Phone.ToUpper().Contains(model.SearchValue ?? "".ToUpper())
                    || (r.ReceivedByUser.Email.ToUpper().Contains(Uri.UnescapeDataString(model.SearchValue ?? "").ToUpper())
                    )))
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => _.Status == ReceiptStatus.Pending && !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var receipts = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            receipts = receipts.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ReceiptModel>(receipts);
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

    public async Task<ResultModel> GetWeeklyReport()
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var days = MyFunction.Get7DaysWithToday();
            days.Reverse();
            var reports = new List<DailyReport>();
            foreach (var day in days)
            {
                var receiptCompleted = _dbContext.Receipt
                    .Where(_ =>
                        new DateTime(_.DateCreated.Year, _.DateCreated.Month, _.DateCreated.Day) == day
                        && _.Status == ReceiptStatus.Completed
                        && !_.IsDeleted).ToList();
                var receiptPending = _dbContext.Receipt
                    .Where(_ =>
                        new DateTime(_.DateCreated.Year, _.DateCreated.Month, _.DateCreated.Day) == day
                        && _.Status == ReceiptStatus.Pending
                        && !_.IsDeleted).ToList();
                var report = new DailyReport()
                {
                    Date = day,
                    TotalCompleted = receiptCompleted.Count(),
                    TotalQuantityCompleted = receiptCompleted.Sum(_ => _.Quantity),
                    TotalPending = receiptPending.Count(),
                    TotalQuantityPending = receiptPending.Sum(_ => _.Quantity),
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

    public async Task<ResultModel> Update(ReceiptUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Receipt.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Receipt not exists";
                result.Succeed = false;
                return result;
            }

            if (data.Status == ReceiptStatus.Completed)
            {
                result.ErrorMessage = "Receipt is complete, cannot be updated";
                result.Succeed = false;
                return result;
            }

            if (model.Note != null)
            {
                data!.Note = model.Note;
            }
            if (model!.Quantity != null)
            {
                data!.Quantity = (int)model.Quantity;
            }
            if (model?.CostPrice != null)
            {
                data!.CostPrice = model.CostPrice;
            }

            data!.DateUpdated = DateTime.Now;
            _dbContext.Receipt.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<Receipt, ReceiptModel>(data);
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
            var data = _dbContext.Receipt.Include(_ => _.Product).Where(_ => _.Id == id).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Receipt not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = _mapper.Map<ReceiptModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
