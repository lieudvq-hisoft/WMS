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

public interface IStockPickingService
{
    Task<ResultModel> Get(PagingParam<SortStockPickingCriteria> paginationModel);
    Task<ResultModel> Create(StockPickingCreate model, Guid createId);
    Task<ResultModel> Update(StockPickingUpdate model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetStockPickingIncoming(PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId);
    Task<ResultModel> GetStockPickingInternal(PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId);
    Task<ResultModel> GetStockPickingOutgoing(PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId);

}
public class StockPickingService : IStockPickingService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public StockPickingService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Get(PagingParam<SortStockPickingCriteria> paginationModel)
    {
        var result = new ResultModel();
        try
        {
            var stockPickings = _dbContext.StockPicking.AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockPickings.Count());
            stockPickings = stockPickings.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockPickings = stockPickings.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<StockPickingModel>(stockPickings);
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

    public async Task<ResultModel> Create(StockPickingCreate model, Guid createUid)
    {
        var result = new ResultModel();
        try
        {
            var stockPicking = _mapper.Map<StockPickingCreate, StockPicking>(model);
            stockPicking.CreateUid = createUid;
            _dbContext.Add(stockPicking);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = stockPicking.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(StockPickingUpdate model)
    {
        var result = new ResultModel();
        try
        {
            var stockPicking = _dbContext.StockPicking.FirstOrDefault(_ => _.Id == model.Id);
            if (stockPicking == null)
            {
                throw new Exception("Stock Picking not exists");
            }
            if(stockPicking.State == PickingState.Done)
            {
                throw new Exception("You cannot update a stock picking that has been set to 'Done'.");

            }
            if (model.LocationId != null)
            {
                stockPicking.LocationId = (Guid)model.LocationId;
            }
            if (model.LocationDestId != null)
            {
                stockPicking.LocationDestId = (Guid)model.LocationDestId;
            }
            if (model.PartnerId != null)
            {
                stockPicking.PartnerId = model.PartnerId;
            }
            if (model.Name != null)
            {
                stockPicking.Name = model.Name;
            }
            if (model.Note != null)
            {
                stockPicking.Note = model.Note;
            }
            if (model.ScheduledDate != null)
            {
                stockPicking.ScheduledDate = model.ScheduledDate;
            }
            if (model.DateDeadline != null)
            {
                stockPicking.DateDeadline = model.DateDeadline;
            }
            stockPicking.WriteDate = DateTime.Now;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockPicking, StockPickingModel>(stockPicking);
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
        try
        {
            var stockPicking = _dbContext.StockPicking.FirstOrDefault(_ => _.Id == id);
            if (stockPicking == null)
            {
                throw new Exception("Stock Picking not exists");
            }
            if (stockPicking.State == PickingState.Done)
            {
                throw new Exception("You cannot delete a stock picking that has been set to 'Done'.");

            }
            _dbContext.Remove(stockPicking);
            result.Succeed = true;
            result.Data = "Deleted successfully!";
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetStockPickingIncoming(PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId)
    {
        var result = new ResultModel();
        try
        {
            var stockPickings = _dbContext.StockPicking
                .Include(_ => _.PickingType)
                .ThenInclude(_ => _.Warehouse)
                .Where(_ => _.PickingType.WarehouseId == warehouseId && _.PickingType.Code == StockPickingTypeCode.Incoming).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockPickings.Count());
            stockPickings = stockPickings.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockPickings = stockPickings.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<StockPickingModel>(stockPickings);
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

    public async Task<ResultModel> GetStockPickingInternal(PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId)
    {
        var result = new ResultModel();
        try
        {
            var stockPickings = _dbContext.StockPicking
                .Include(_ => _.PickingType)
                .ThenInclude(_ => _.Warehouse)
                .Where(_ => _.PickingType.WarehouseId == warehouseId && _.PickingType.Code == StockPickingTypeCode.Incoming).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockPickings.Count());
            stockPickings = stockPickings.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockPickings = stockPickings.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<StockPickingModel>(stockPickings);
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

    public async Task<ResultModel> GetStockPickingOutgoing(PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId)
    {
        var result = new ResultModel();
        try
        {
            var stockPickings = _dbContext.StockPicking
                .Include(_ => _.PickingType)
                .ThenInclude(_ => _.Warehouse)
                .Where(_ => _.PickingType.WarehouseId == warehouseId && _.PickingType.Code == StockPickingTypeCode.Incoming).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockPickings.Count());
            stockPickings = stockPickings.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockPickings = stockPickings.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<StockPickingModel>(stockPickings);
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
