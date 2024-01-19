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
using static Confluent.Kafka.ConfigPropertyNames;

namespace Services.Core;

public interface IProductService
{
    Task<ResultModel> Create(ProductCreateModel model);
    Task<ResultModel> Update(ProductUpdateModel model);
    Task<ResultModel> Get(PagingParam<ProductSortCriteria> paginationModel, ProductSearchModel model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetInventoryQuantity(Guid id);
    Task<ResultModel> GetInventories(Guid id);
    Task<ResultModel> GetPickingRequestCompleted(Guid id);
    Task<ResultModel> GetPickingRequestPending(Guid id);
    Task<ResultModel> GetQrcode(Guid id);
    Task<ResultModel> GetBarcode(Guid id);
    Task<ResultModel> GetDetail(Guid id);
    Task<ResultModel> UploadImg(UploadImgModel model);
    Task<ResultModel> DeleteImg(DeleteImgModel model);
    Task<ResultModel> GetReportInventory();
    void InventoryThresholdWarning();
}
public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IProducer<Null, string> _producer;

    public ProductService(AppDbContext dbContext, IMapper mapper, IProducer<Null, string> producer
        )
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _producer = producer;
    }

    public async Task<ResultModel> Create(ProductCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _mapper.Map<ProductCreateModel, Product>(model);
            _dbContext.Product.Add(data);
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
            var data = _dbContext.Product.Include(_ => _.Inventories).Where(delegate (Product p)
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

            var userReceiveNotice = _dbContext.User.Include(_ => _.UserRoles).ThenInclude(_ => _.Role)
                .Where(_ => _.UserRoles.Any(ur => ur.Role.NormalizedName != "ADMIN") && _.IsActive && !_.IsDeleted)
                .Select(_ => _.Id).ToList();
            var kafkaModel = new KafkaModel { UserReceiveNotice = userReceiveNotice, Payload = _mapper.Map<Product, ProductModel>(data) };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(kafkaModel);
            await _producer.ProduceAsync("product-update", new Message<Null, string> { Value = json });
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
                product.Images.Add(await MyFunction.uploadFileAsync(model.File, dirPath, "/app/wwwroot"));
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
                MyFunction.deleteFile(dirPath + model.Path);
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

    public async void InventoryThresholdWarning()
    {
        try
        {
            var products = _dbContext.Product.Include(_ => _.Inventories).Where(_ => !_.IsDeleted).ToList();
            var inventoryThreshold = _dbContext.InventoryThresholds.FirstOrDefault();
            if (products.Any())
            {
                var userReceiveNotice = _dbContext.User.Include(_ => _.UserRoles).ThenInclude(_ => _.Role)
                    .Where(_ => _.UserRoles.Any(ur => ur.Role.NormalizedName != "STAFF") && _.IsActive && !_.IsDeleted)
                    .Select(_ => _.Id).ToList();
                foreach (var product in products)
                {
                    var inventories = product.Inventories.Where(_ => _.IsAvailable && !_.IsDeleted).ToList();
                    if (inventories.Count() <= inventoryThreshold!.ThresholdQuantity)
                    {
                        var kafkaModel = new KafkaModel { UserReceiveNotice = userReceiveNotice, Payload = _mapper.Map<Product, ProductModel>(product) };
                        var json = Newtonsoft.Json.JsonConvert.SerializeObject(kafkaModel);
                        await _producer.ProduceAsync("inventory-threshold-warning", new Message<Null, string> { Value = json });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
    }
}
