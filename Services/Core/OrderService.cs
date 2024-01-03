using AutoMapper;
using Data.Common.PaginationModel;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Data.Models;
using Data.Utils.Paging;
using Microsoft.EntityFrameworkCore;
using Services.Utils;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Services.Core;

public interface IOrderService
{
    Task<ResultModel> Create(OrderCreateModel model, Guid userId);
    Task<ResultModel> Update(ProductUpdateModel model);
    Task<ResultModel> Get(PagingParam<ProductSortCriteria> paginationModel, ProductSearchModel model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetInventories(Guid id);
    Task<ResultModel> GetPickingRequestCompleted(Guid id);
    Task<ResultModel> GetPickingRequestPending(Guid id);
    Task<ResultModel> GetDetail(Guid id);
    Task<ResultModel> UploadImg(UploadImgModel model);
    Task<ResultModel> DeleteImg(DeleteImgModel model);

}
public class OrderService : IOrderService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public OrderService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> Create(OrderCreateModel model, Guid userId)
    {
        var result = new ResultModel();
        result.Succeed = false;
        var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var order = _mapper.Map<OrderCreateModel, Order>(model);
            order.Files = new List<string>();
            order.SentBy = userId;
            _dbContext.Order.Add(order);

            if (model.PickingRequests!.Any())
            {
                var pickingRequests = _mapper.Map<List<PickingRequestInnerCreateModel>, List<PickingRequest>>(model.PickingRequests!)
                    .Select(item =>
                    {
                        item.OrderId = order.Id;
                        return item;
                    });
                _dbContext.PickingRequest.AddRange(pickingRequests);
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
        return result;
    }

    public async Task<ResultModel> Delete(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.Product.Update(data);
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

    public async Task<ResultModel> Get(PagingParam<ProductSortCriteria> paginationModel, ProductSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Where(delegate (Product p)
            {
                if (
                    (MyFunction.ConvertToUnSign(p.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (p.SerialNumber.ToUpper().Contains(model.SearchValue ?? "".ToUpper()))
                    )
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var products = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            products = products.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ProductModel>(products);
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

    public async Task<ResultModel> GetQrcode(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = MyFunction.GenerateQrcode(data.Id.ToString());
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetBarcode(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = MyFunction.GenerateBarcode(content: data.Id.ToString());
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
            var data = _dbContext.Product.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = _mapper.Map<ProductModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetInventories(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product
                .Include(_ => _.Inventories).ThenInclude(_ => _.InventoryLocations).ThenInclude(_ => _.Location).ThenInclude(_ => _.RackLevel).ThenInclude(_ => _.Rack)
                .Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            var inventories = data.Inventories.Where(_ => _.IsAvailable && !_.IsDeleted).OrderBy(_ => _.DateCreated).AsQueryable();
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<InventoryFPModel>(inventories);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetInventoryLocation(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Include(_ => _.Inventories)
                .ThenInclude(_ => _.InventoryLocations).ThenInclude(_ => _.Location).ThenInclude(_ => _.RackLevel).ThenInclude(_ => _.Rack)
                .Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            var inventories = data.Inventories.Where(_ => _.QuantityOnHand > 0 && !_.IsDeleted).OrderBy(_ => _.DateCreated).AsQueryable();
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<InventoryModel>(inventories);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetInventoryQuantity(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Include(_ => _.Inventories).Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            var quantity = 0;
            foreach (var item in data.Inventories)
            {
                if(item.QuantityOnHand > 0)
                {
                    quantity += item.QuantityOnHand;
                }
            }
            result.Succeed = true;
            result.Data = quantity;

        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetPickingRequestCompleted(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Include(_ => _.PickingRequests).ThenInclude(_ => _.PickingRequestUsers).ThenInclude(_ => _.ReceivedByUser).Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            var pickingRequests = data.PickingRequests.Where(_ => _.Status == PickingRequestStatus.Completed && !_.IsDeleted).AsQueryable();
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<PickingRequestModel>(pickingRequests);

        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetPickingRequestPending(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Include(_ => _.PickingRequests).ThenInclude(_ => _.PickingRequestUsers).ThenInclude(_ => _.ReceivedByUser).Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            var pickingRequests = data.PickingRequests.Where(_ => _.Status == PickingRequestStatus.Pending && !_.IsDeleted).AsQueryable();
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<PickingRequestModel>(pickingRequests);

        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(ProductUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            if (model.Name != null)
            {
                data!.Name = model.Name;
            }

            if (model.Description != null)
            {
                data!.Description = model.Description;
            }

            if (model.SalePrice != null)
            {
                data!.SalePrice = model.SalePrice;
            }

            if (model.SerialNumber != null)
            {
                data!.SerialNumber = model.SerialNumber;
            }
            if (model.InternalCode != null)
            {
                data!.InternalCode = model.InternalCode;
            }
            data!.DateUpdated = DateTime.Now;
            _dbContext.Product.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<Product, ProductModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UploadImg(UploadImgModel model)
    {
        var result = new ResultModel();
        try
        {
            var product = _dbContext.Product.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (product == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Product not found";
            }
            else
            {
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "product");
                if (product.Images == null)
                {
                    product.Images = new List<string>();
                }
                product.Images.Add(await MyFunction.uploadImageAsync(model.File, dirPath));
                product.DateUpdated = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                result.Data = product.Images;
                result.Succeed = true;
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }

        return result;
    }

    public async Task<ResultModel> DeleteImg(DeleteImgModel model)
    {
        var result = new ResultModel();
        try
        {
            var product = _dbContext.Product.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (product == null)
            {
                result.Succeed = false;
                result.ErrorMessage = "Product not found";
            }
            else
            {
                string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                if (product.Images == null || !product.Images.Contains(model.Path))
                {
                    result.ErrorMessage = "Image does not exist";
                    result.Succeed = false;
                    return result;
                }
                MyFunction.deleteImage(dirPath + model.Path);
                product.Images.Remove(model.Path);
                product.DateUpdated = DateTime.Now;
                await _dbContext.SaveChangesAsync();
                result.Data = product.Images;
                result.Succeed = true;
            }
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetReportInventory()
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product
                .Include(_ => _.Inventories).Where(_ => !_.IsDeleted).Select(_ => new ProductInventoryModel
                { SerialNumber = _.SerialNumber,
                  Name = _.Name,
                  totalInventory = _.Inventories.Where(i => i.IsAvailable).Count()
                }).ToList();
            result.Succeed = true;
            result.Data = data;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
