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

}
public class StockLocationService : IStockLocationService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly Guid _virtualLocationId = new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071");
    private readonly Guid _inventoryAdjustmentId = new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6");
    private readonly Guid _physicalLocationId = new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc");
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

}
