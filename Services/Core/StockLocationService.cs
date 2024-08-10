using System;
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

namespace Services.Core;

public interface IStockLocationService
{
    Task<ResultModel> Get(PagingParam<StockLocationSortCriteria> paginationModel);
    Task<ResultModel> GetInfo(Guid id);
    Task<ResultModel> GetForSelectParent(Guid id);
    Task<ResultModel> GetInternalLocation();
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetStockQuant(PagingParam<StockQuantSortCriteria> paginationModel, Guid id);
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
            var stockLocations = _dbContext.StockLocation.AsQueryable();
            if (!string.IsNullOrEmpty(paginationModel.SearchText))
            {
                stockLocations = stockLocations.Where(_ => _.Name.Contains(paginationModel.SearchText));
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

    public async Task<ResultModel> GetInternalLocation()
    {
        var result = new ResultModel();
        try
        {
            var productCategories = _dbContext.StockLocation.Where(_ => _.Usage == LocationType.Internal).AsQueryable().OrderBy(_ => _.CompleteName);
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<StockLocationModel>(productCategories).ToList();
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

}
