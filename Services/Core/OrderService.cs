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

public interface IOrderService
{
    Task<ResultModel> Create(OrderCreateModel model, Guid userId);
    Task<ResultModel> Complete(OrderCompleteModel model);
    Task<ResultModel> Update(OrderUpdateModel model);
    Task<ResultModel> Get(PagingParam<OrderSortCriteria> paginationModel, OrderSearchModel model);
    Task<ResultModel> GetDetail(Guid id);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> UploadFile(UploadFileModel model);
    Task<ResultModel> DeleteFile(FileModel model);
    Task<ResultModel> DownloadFile(FileModel model);
}
public class OrderService : IOrderService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IProducer<Null, string> _producer;

    public OrderService(AppDbContext dbContext, IMapper mapper, IProducer<Null, string> producer)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _producer = producer;
    }

    public async Task<ResultModel> Create(OrderCreateModel model, Guid userId)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = _dbContext.Database.BeginTransaction())
        {
            try
            {
                var order = new Order { Note = model.Note };
                order.Files = new List<string>();
                order.SentBy = userId;
                _dbContext.Order.Add(order);

                if (model.PickingRequests!.Any())
                {
                    foreach (var item in model.PickingRequests!)
                    {
                        var product = _dbContext.Product.Where(_ => _.Id == item.ProductId && !_.IsDeleted).FirstOrDefault();
                        if (product == null)
                        {
                            result.ErrorMessage = "Product not exists with productId = " + item.ProductId.ToString();
                            result.Succeed = false;
                            await transaction.RollbackAsync();
                            return result;
                        }
                        if (item.Quantity <= 0)
                        {
                            result.ErrorMessage = "Picking request quantity than 0";
                            result.Succeed = false;
                            await transaction.RollbackAsync();
                            return result;
                        }
                        var pickingRequest = _mapper.Map<PickingRequestInnerCreateModel, PickingRequest>(item);
                        pickingRequest.OrderId = order.Id;
                        _dbContext.PickingRequest.Add(pickingRequest);
                    }
                }
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = order.Id;
                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                await transaction.RollbackAsync();
            }
        }
        //var transaction = _dbContext.Database.BeginTransaction();

        return result;
    }

    public async Task<ResultModel> Complete(OrderCompleteModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var order = _dbContext.Order
                .Include(_ => _.SentByUser)
                .Include(_ => _.PickingRequests).Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (order == null)
            {
                result.ErrorMessage = "Order not exists";
                result.Succeed = false;
                return result;
            }
            if (order.Status == OrderStatus.Completed)
            {
                result.ErrorMessage = "Order is completed";
                result.Succeed = false;
                return result;
            }
            if (!order.PickingRequests.Any(_ => _.Status != PickingRequestStatus.Completed && !_.IsDeleted))
            {
                order.Status = OrderStatus.Completed;
                order.DateUpdated = DateTime.Now;
                _dbContext.Order.Update(order);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = order.Id;

                var kafkaModel = new KafkaModel { UserReceiveNotice = new List<Guid>() { order.SentBy }, Payload = _mapper.Map<Order, OrderInnerModel>(order!) };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(kafkaModel);
                await _producer.ProduceAsync("order-complete", new Message<Null, string> { Value = json });
            }
            else
            {
                result.ErrorMessage = "Order cannot be completed because it is related to an existing picking request pending";
            }
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
            var order = _dbContext.Order.Include(_ => _.PickingRequests).Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (order == null)
            {
                result.ErrorMessage = "Order not exists";
                result.Succeed = false;
                return result;
            }
            if (!order.PickingRequests.Any(_ => !_.IsDeleted))
            {
                order.IsDeleted = true;
                order.DateUpdated = DateTime.Now;
                _dbContext.Order.Update(order);
                await _dbContext.SaveChangesAsync();
                result.Succeed = true;
                result.Data = order.Id;
            }else
            {
                result.ErrorMessage = "Order cannot be deleted because it is related to an existing picking request";
            }

        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(OrderUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var order = _dbContext.Order.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (order == null)
            {
                result.ErrorMessage = "Order not exists";
                result.Succeed = false;
                return result;
            }
            if (model.Note != null)
            {
                order.Note = model.Note;
            }
            order.DateUpdated = DateTime.Now;
            _dbContext.Order.Update(order);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = order.Id;

        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Get(PagingParam<OrderSortCriteria> paginationModel, OrderSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Order
                .Include(_ => _.SentByUser)
                .Include(_ => _.PickingRequests).ThenInclude(_ => _.Product).ThenInclude(_ => _.Inventories)
                .Where(delegate (Order o)
            {
                if (
                    (MyFunction.ConvertToUnSign(o.SentByUser.UserName ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    )
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var orders = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            orders = orders.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<OrderModel>(orders);
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

    public async Task<ResultModel> GetDetail(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Order
                .Include(_ => _.PickingRequests).ThenInclude(_ => _.Product).ThenInclude(_ => _.Inventories)
                .Include(_ => _.SentByUser).Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Order not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = _mapper.Map<OrderModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UploadFile(UploadFileModel model)
    {
        var result = new ResultModel();
        try
        {
            var order = _dbContext.Order.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (order == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Order not found";
            }
            else
            {
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "document", "order", order.Id.ToString());
                if (order.Files == null)
                {
                    order.Files = new List<string>();
                }
                order.Files.Add(await MyFunction.uploadFileAsync(model.File, dirPath, "/app/document"));
                order.DateUpdated = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                result.Data = order.Files;
                result.Succeed = true;
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }

        return result;
    }

    public async Task<ResultModel> DeleteFile(FileModel model)
    {
        var result = new ResultModel();
        try
        {
            var order = _dbContext.Order.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (order == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Order not found";
            }
            else
            {
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "document");
                if (order.Files == null || !order.Files.Contains(model.Path))
                {
                    result.ErrorMessage = "File does not exist";
                    result.Succeed = false;
                    return result;
                }
                MyFunction.deleteFile(dirPath + model.Path);
                order.Files.Remove(model.Path);
                order.DateUpdated = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                result.Data = order.Files;
                result.Succeed = true;
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> DownloadFile(FileModel model)
    {
        var result = new ResultModel();
        try
        {
            var order = _dbContext.Order.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (order == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Order not found";
            }
            else
            {
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "document");
                if (order.Files == null || !order.Files.Contains(model.Path))
                {
                    result.ErrorMessage = "File does not exist";
                    result.Succeed = false;
                    return result;
                }
                result.Data = await MyFunction.downloadFile(dirPath + model.Path);
                result.Succeed = true;
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

}
