using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Data.Common.PaginationModel;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Data.Models;
using Data.Utils.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Services.Core;

public interface IStockLocationService
{
    Task<ResultModel> Get(PagingParam<StockLocationSortCriteria> paginationModel);
    Task<ResultModel> GetInfo(Guid id);
    Task<ResultModel> GetForSelectParent(Guid id);
    Task<ResultModel> GetForSelectParent();
    Task<ResultModel> GetLocation();
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetStockQuant(PagingParam<StockQuantSortCriteria> paginationModel, Guid id);
    Task<ResultModel> Create(StockLocationCreate model);
    Task<ResultModel> UpdateParent(StockLocationParentUpdate model);
    Task<ResultModel> Update(StockLocationUpdate model);
    Task<ResultModel> GetLocationWarehouse(Guid warehouseId);
}
public class StockLocationService : IStockLocationService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly Guid _virtualLocationId = new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071");
    private readonly Guid _inventoryAdjustmentId = new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6");
    private readonly Guid _physicalLocationId = new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc");
    private readonly Guid _partnerLocationId = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479");
    private readonly Guid _vendorLocationId = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
    private readonly Guid _customerLocationId = new Guid("6ba7b180-9cad-11d1-80b4-00c04fd430c8");
    public StockLocationService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Get(PagingParam<StockLocationSortCriteria> paginationModel)
    {
        var result = new ResultModel();
        try
        {
            var stockLocations = _dbContext.StockLocation.Include(_ => _.StockQuants).AsQueryable();
            if (!string.IsNullOrEmpty(paginationModel.SearchText))
            {
                stockLocations = stockLocations.Where(_ => _.CompleteName.Contains(paginationModel.SearchText));
            }
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockLocations.Count());
            stockLocations = stockLocations.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockLocations = stockLocations.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<StockLocationModel>(stockLocations);
            paging.Data = viewModels;
            result.Succeed = true;
            result.Data = paging;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetInfo(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockLocation = _dbContext.StockLocation.Include(_ => _.ParentLocation).FirstOrDefault(_ => _.Id == id);
            if (stockLocation == null)
            {
                throw new Exception("Stock location not exists");
            }
            result.Succeed = true;
            result.Data = _mapper.Map<StockLocationInfo>(stockLocation);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetForSelectParent(Guid id)
    {
        var result = new ResultModel();
        try
        {
            var productCategories = _dbContext.StockLocation.Where(_ => _.Id != id).AsQueryable().OrderBy(_ => _.CompleteName);
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<StockLocationModel>(productCategories).ToList();
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetForSelectParent()
    {
        var result = new ResultModel();
        try
        {
            var productCategories = _dbContext.StockLocation.AsQueryable().OrderBy(_ => _.CompleteName);
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<StockLocationModel>(productCategories).ToList();
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetLocation()
    {
        var result = new ResultModel();
        try
        {
            var productCategories = _dbContext.StockLocation
                .Where(_ =>
                //_.Usage == LocationType.Internal
                _.Id != _virtualLocationId && _.Id != _inventoryAdjustmentId &&  _.Id != _physicalLocationId && _.Id != _partnerLocationId && _.Id != _vendorLocationId && _.Id != _customerLocationId
            ).AsQueryable().OrderBy(_ => _.CompleteName);
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<StockLocationModel>(productCategories).ToList();
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetLocationWarehouse(Guid warehouseId)
    {
        var result = new ResultModel();
        try
        {
            var warehouse = _dbContext.StockWarehouse.Include(_ => _.ViewLocation).FirstOrDefault(_ => _.Id == warehouseId);
            if (warehouse == null)
            {
                throw new Exception("Warehouse not exists");
            }
            var viewLocationParentPath = warehouse.ViewLocation.ParentPath;
            var stockLocations = _dbContext.StockLocation
                .Where(_ =>
                _.ParentPath.StartsWith(viewLocationParentPath) &&
                _.Id != _virtualLocationId && _.Id != _inventoryAdjustmentId && _.Id != _physicalLocationId && _.Id != _partnerLocationId && _.Id != _vendorLocationId && _.Id != _customerLocationId
            ).AsQueryable().OrderBy(_ => _.CompleteName);
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<StockLocationModel>(stockLocations).ToList();
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
            var stockLocation = _dbContext.StockLocation.FirstOrDefault(_ => _.Id == id);
            if (stockLocation == null)
            {
                throw new Exception("Stock location not exists");
            }

            if (stockLocation.Id == _virtualLocationId || stockLocation.Id == _inventoryAdjustmentId || stockLocation.Id == _physicalLocationId || stockLocation.Id == _partnerLocationId || stockLocation.Id == _vendorLocationId || stockLocation.Id == _customerLocationId)
            {
                throw new Exception("Unable to delete this document because it is used as a default property");
            }
            _dbContext.Remove(stockLocation);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = "Deleted successfully!";
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetStockQuant(PagingParam<StockQuantSortCriteria> paginationModel, Guid id)
    {
        var result = new ResultModel();
        try
        {
            var stockQuants = _dbContext.StockQuant
                .Include(sq => sq.StockLocation)
                .Include(sq => sq.ProductProduct)
                    .ThenInclude(pp => pp.ProductTemplate)
                    .ThenInclude(pt => pt.UomUom)
                 .Include(sq => sq.ProductProduct)
                    .ThenInclude(pp => pp.ProductVariantCombinations)
                .Where(sq => sq.LocationId == id && sq.StockLocation.Usage == LocationType.Internal)
                .AsQueryable();
            if (!string.IsNullOrEmpty(paginationModel.SearchText))
            {
                stockQuants = stockQuants.Where(_ => _.ProductProduct.ProductTemplate.Name.Contains(paginationModel.SearchText));
            }
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockQuants.Count());
            stockQuants = stockQuants.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockQuants = stockQuants.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = stockQuants
                .Select(_ => new StockQuantInfo
                {
                    Id = _.Id,
                    ProductProduct = new ProductProductModel
                    {
                        Id = _.ProductId,
                        Name = _.ProductProduct.ProductTemplate.Name,
                        Pvcs = _.ProductProduct.ProductVariantCombinations.OrderBy(pvc => pvc.CreateDate).Select(pvc =>
                            new Pvc
                            {
                                Attribute = pvc.ProductTemplateAttributeValue.ProductAttributeValue.ProductAttribute.Name,
                                Value = pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name
                            })
                            .ToList()
                    },
                    StockLocation = _mapper.Map<StockLocationModel>(_.StockLocation),
                    InventoryDate = _.InventoryDate,
                    Quantity = _.Quantity,
                    UomUom = _.ProductProduct.ProductTemplate.UomUom.Name,
                    InventoryQuantity = _.InventoryQuantity,
                    InventoryDiffQuantity = _.InventoryDiffQuantity,
                    InventoryQuantitySet = _.InventoryQuantitySet,
                });
            paging.Data = viewModels;
            result.Succeed = true;
            result.Data = paging;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Create(StockLocationCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockLocation = _mapper.Map<StockLocationCreate, StockLocation>(model);
            if (stockLocation.LocationId != null)
            {
                var parentStockLocation = _dbContext.StockLocation.FirstOrDefault(_ => _.Id == stockLocation.LocationId);
                if (parentStockLocation == null)
                {
                    throw new Exception("Parent Location not exists");
                }
                _dbContext.Add(stockLocation);
                stockLocation.CompleteName = $"{parentStockLocation.CompleteName} / {stockLocation.Name}";
                stockLocation.ParentPath = $"{parentStockLocation.ParentPath}/{stockLocation.Id}";
            }
            else
            {
                _dbContext.Add(stockLocation);
                stockLocation.CompleteName = stockLocation.Name;
                stockLocation.ParentPath = stockLocation.Id.ToString();
            }
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockLocation, StockLocationModel>(stockLocation);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateParent(StockLocationParentUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var newParentStockLocation = _dbContext.StockLocation.FirstOrDefault(_ => _.Id == model.ParentId);
                if (newParentStockLocation == null)
                {
                    throw new Exception("New parent Location not exists");
                }
                var stockLocation = _dbContext.StockLocation.Include(_ => _.StockQuants).FirstOrDefault(_ => _.Id == model.Id);
                if (stockLocation == null)
                {
                    throw new Exception("Location not exists");
                }
                if (stockLocation.Id == _virtualLocationId || stockLocation.Id == _inventoryAdjustmentId || stockLocation.Id == _physicalLocationId || stockLocation.Id == _partnerLocationId || stockLocation.Id == _vendorLocationId || stockLocation.Id == _customerLocationId)
                {
                    throw new Exception("Unable to update this document because it is used as a default property");
                }

                var locationDocument = _dbContext.StockWarehouse.FirstOrDefault(_ => _.ViewLocationId == stockLocation.Id || _.LotStockId == stockLocation.Id);
                if(locationDocument != null)
                {
                    throw new Exception("Unable to update this document because it is used as a default property");
                }

                if (newParentStockLocation.ParentPath.Contains(stockLocation.Id.ToString()))
                {
                    throw new InvalidOperationException($"Detected a cyclic dependency involving ID {stockLocation.Id.ToString()}. Update aborted to prevent infinite recursion.");
                }
                stockLocation.LocationId = newParentStockLocation.Id;
                await _dbContext.SaveChangesAsync();

                ComputeCompleteNameAndParentPath(stockLocation);
                UpdateCompleteNameAndParentPathRecursive(_dbContext, stockLocation.Id);

                stockLocation.WriteDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                result.Succeed = true;
                result.Data = _mapper.Map<StockLocation, StockLocationModel>(stockLocation);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                await transaction.RollbackAsync();
            }
        }
        return result;
    }

    public async Task<ResultModel> Update(StockLocationUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var stockLocation = _dbContext.StockLocation
                    .Include(_ => _.StockQuants)
                    .Include(_ => _.ParentLocation).FirstOrDefault(_ => _.Id == model.Id);
                if (stockLocation == null)
                {
                    throw new Exception("Location not exists");
                }
                if (stockLocation.Id == _virtualLocationId || stockLocation.Id == _inventoryAdjustmentId || stockLocation.Id == _physicalLocationId || stockLocation.Id == _partnerLocationId || stockLocation.Id == _vendorLocationId || stockLocation.Id == _customerLocationId)
                {
                    throw new Exception("Unable to update this document because it is used as a default property");
                }
                var locationDocument = _dbContext.StockWarehouse.FirstOrDefault(_ => _.ViewLocationId == stockLocation.Id || _.LotStockId == stockLocation.Id);
                if (locationDocument != null)
                {
                    throw new Exception("Unable to update this document because it is used as a default property");
                }
                if (model.Name != null)
                {
                    stockLocation.Name = model.Name;
                    ComputeCompleteNameAndParentPath(stockLocation);
                    UpdateCompleteNameAndParentPathRecursive(_dbContext, stockLocation.Id);
                }
                if (model.Usage != null)
                {
                    stockLocation.Usage = (LocationType)model.Usage;
                }
                stockLocation.WriteDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                result.Succeed = true;
                result.Data = _mapper.Map<StockLocation, StockLocationModel>(stockLocation);

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                await transaction.RollbackAsync();
            }
        }
        return result;
    }

    private async void ComputeCompleteNameAndParentPath(StockLocation stockLocation)
    {
        if (stockLocation.ParentLocation != null)
        {
            stockLocation.CompleteName = $"{stockLocation.ParentLocation.CompleteName} / {stockLocation.Name}";
            stockLocation.ParentPath = $"{stockLocation.ParentLocation.ParentPath}/{stockLocation.Id}";
        }
        else
        {
            stockLocation.CompleteName = stockLocation.Name;
            stockLocation.ParentPath = stockLocation.Id.ToString();
        }
    }

    private async void UpdateCompleteNameAndParentPathRecursive(AppDbContext dbContext, Guid Id)
    {
        foreach (var child in dbContext.StockLocation.Include(_ => _.ParentLocation).Where(c => c.LocationId == Id).ToList())
        {
            if (child.ParentLocation != null)
            {
                child.CompleteName = $"{child.ParentLocation.CompleteName} / {child.Name}";
                child.ParentPath = $"{child.ParentLocation.ParentPath}/{child.Id}";
            }

            UpdateCompleteNameAndParentPathRecursive(dbContext, child.Id);
        }
    }

}
