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

public interface IStockWarehouseService
{
    Task<ResultModel> Create(StockWarehouseCreate model);
    Task<ResultModel> Update(StockWarehouseUpdate model);
    Task<ResultModel> Get(PagingParam<StockWarehouseSortCriteria> paginationModel);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetInfo(Guid id);
}
public class StockWarehouseService : IStockWarehouseService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly Guid _virtualLocationId = new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071");
    private readonly Guid _inventoryAdjustmentId = new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6");
    private readonly Guid _physicalLocationId = new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc");
    public StockWarehouseService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(StockWarehouseCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {

                var stockWarehouse = _mapper.Map<StockWarehouseCreate, StockWarehouse>(model);

                var viewLocationId = Guid.NewGuid();
                var viewLocation = new StockLocation
                {
                    Id = viewLocationId,
                    Name = stockWarehouse.Code,
                    CompleteName = stockWarehouse.Code,
                    ParentPath = $"{_physicalLocationId}/{viewLocationId}/",
                    Usage = LocationType.View,
                };
                _dbContext.Add(viewLocation);

                var lotStockId = Guid.NewGuid();
                var lotStock = new StockLocation
                {
                    Id = lotStockId,
                    LocationId = viewLocationId,
                    Name = "Stock",
                    CompleteName = $"{viewLocation.CompleteName}/Stock",
                    ParentPath = $"{viewLocation.ParentPath}{lotStockId}",
                    Usage = LocationType.Internal,
                };
                _dbContext.Add(lotStock);

                var whInputStockLocId = Guid.NewGuid();
                var whInputStockLoc = new StockLocation
                {
                    Id = whInputStockLocId,
                    LocationId = viewLocationId,
                    Name = "Input",
                    CompleteName = $"{viewLocation.CompleteName}/Input",
                    ParentPath = $"{viewLocation.ParentPath}{whInputStockLocId}",
                    Usage = LocationType.Internal,
                };
                _dbContext.Add(whInputStockLoc);

                var whQcStockLocId = Guid.NewGuid();
                var whQcStockLoc = new StockLocation
                {
                    Id = whQcStockLocId,
                    LocationId = viewLocationId,
                    Name = "Quality Control",
                    CompleteName = $"{viewLocation.CompleteName}/Quality Control",
                    ParentPath = $"{viewLocation.ParentPath}{whQcStockLocId}",
                    Usage = LocationType.Internal,
                };
                _dbContext.Add(whQcStockLoc);

                var whOutputStockLocId = Guid.NewGuid();
                var whOutputStockLoc = new StockLocation
                {
                    Id = whOutputStockLocId,
                    LocationId = viewLocationId,
                    Name = "Output",
                    CompleteName = $"{viewLocation.CompleteName}/Output",
                    ParentPath = $"{viewLocation.ParentPath}{whOutputStockLocId}",
                    Usage = LocationType.Internal,
                };
                _dbContext.Add(whOutputStockLoc);

                var whPackStockLocId = Guid.NewGuid();
                var whPackStockLoc = new StockLocation
                {
                    Id = whPackStockLocId,
                    LocationId = viewLocationId,
                    Name = "Packing Zone",
                    CompleteName = $"{viewLocation.CompleteName}/Packing Zone",
                    ParentPath = $"{viewLocation.ParentPath}{whPackStockLocId}",
                    Usage = LocationType.Internal,
                };
                _dbContext.Add(whPackStockLoc);

                stockWarehouse.ViewLocationId = viewLocationId;
                stockWarehouse.LotStockId = lotStockId;
                stockWarehouse.WhInputStockLocId = whInputStockLocId;
                stockWarehouse.WhQcStockLocId = whQcStockLocId;
                stockWarehouse.WhOutputStockLocId = whOutputStockLocId;
                stockWarehouse.WhPackStockLocId = whPackStockLocId;
                _dbContext.Add(stockWarehouse);

                var sptReceipt = new StockPickingType
                {
                    WarehouseId = stockWarehouse.Id,
                    Code = StockPickingTypeCode.Incoming,
                    Barcode = $"{stockWarehouse.Code}-RECEIPTS",
                    Name = "Receipts",
                    CreateBackorder = CreateBackorderType.Ask
                };
                _dbContext.Add(sptReceipt);

                var sptDelivery = new StockPickingType
                {
                    WarehouseId = stockWarehouse.Id,
                    Code = StockPickingTypeCode.Outgoing,
                    Barcode = $"{stockWarehouse.Code}-DELIVERY",
                    Name = "Delivery Orders",
                    CreateBackorder = CreateBackorderType.Ask
                };
                _dbContext.Add(sptDelivery);

                var sptPick = new StockPickingType
                {
                    WarehouseId = stockWarehouse.Id,
                    Code = StockPickingTypeCode.Internal,
                    Barcode = $"{stockWarehouse.Code}-PICK",
                    Name = "Pick",
                    CreateBackorder = CreateBackorderType.Ask
                };
                _dbContext.Add(sptPick);

                var sptPack = new StockPickingType
                {
                    WarehouseId = stockWarehouse.Id,
                    Code = StockPickingTypeCode.Internal,
                    Barcode = $"{stockWarehouse.Code}-PACK",
                    Name = "Pack",
                    CreateBackorder = CreateBackorderType.Ask
                };
                _dbContext.Add(sptPack);

                var sptInternal = new StockPickingType
                {
                    WarehouseId = stockWarehouse.Id,
                    Code = StockPickingTypeCode.Internal,
                    Barcode = $"{stockWarehouse.Code}-INTERNAL",
                    Name = "Internal Transfers",
                    CreateBackorder = CreateBackorderType.Ask
                };
                _dbContext.Add(sptInternal);

                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = _mapper.Map<StockWarehouse, StockWarehouseModel>(stockWarehouse);
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

    public async Task<ResultModel> Update(StockWarehouseUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockWarehouse = _dbContext
                .StockWarehouse
                .Include(_ => _.ViewLocation)
                .Include(_ => _.StockPickingTypes)
                .FirstOrDefault(_ => _.Id == model.Id);
            if (stockWarehouse == null)
            {
                throw new Exception("Warehouse not exists");
            }
            if(model.Name != null)
            {
                stockWarehouse.Name = model.Name;
            }
            if(model.Code != null)
            {
                string oldCode = stockWarehouse.Code;
                stockWarehouse.Code = model.Code;
                stockWarehouse.ViewLocation.Name = model.Code;
                stockWarehouse.ViewLocation.CompleteName = model.Code;
                await _dbContext.SaveChangesAsync();

                UpdateCompleteNameAndParentPathRecursive(_dbContext, stockWarehouse.ViewLocationId);

                foreach (var pickingType in stockWarehouse.StockPickingTypes)
                {
                    if (pickingType.Barcode.StartsWith(oldCode))
                    {
                        pickingType.Barcode = $"{stockWarehouse.Code}{pickingType.Barcode.Substring(oldCode.Length)}";
                    }
                }
            }
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockWarehouseModel>(stockWarehouse);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Get(PagingParam<StockWarehouseSortCriteria> paginationModel)
    {
        var result = new ResultModel();
        try
        {
            var stockWarehouses = _dbContext.StockWarehouse.AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockWarehouses.Count());
            stockWarehouses = stockWarehouses.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockWarehouses = stockWarehouses.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<StockWarehouseModel>(stockWarehouses);
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

    public async Task<ResultModel> Delete(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockWarehouse = _dbContext.StockWarehouse
                .Include(_ => _.ViewLocation)
                .Include(_ => _.LotStock)
                .Include(_ => _.WhInputStockLoc)
                .Include(_ => _.WhOutputStockLoc)
                .Include(_ => _.WhPackStockLoc)
                .Include(_ => _.WhQcStockLoc)
                .FirstOrDefault(_ => _.Id == id);
            if (stockWarehouse == null)
            {
                throw new Exception("Warehouse not exists");
            }
            _dbContext.StockLocation.Remove(stockWarehouse.ViewLocation);
            _dbContext.StockLocation.Remove(stockWarehouse.LotStock);
            _dbContext.StockLocation.Remove(stockWarehouse.WhInputStockLoc);
            _dbContext.StockLocation.Remove(stockWarehouse.WhOutputStockLoc);
            _dbContext.StockLocation.Remove(stockWarehouse.WhPackStockLoc);
            _dbContext.StockLocation.Remove(stockWarehouse.WhQcStockLoc);
            _dbContext.Remove(stockWarehouse);
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

    public async Task<ResultModel> GetInfo(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockWarehouse = _dbContext.StockWarehouse
                .Include(_ => _.ViewLocation)
                .Include(_ => _.LotStock)
                .Include(_ => _.WhInputStockLoc)
                .Include(_ => _.WhOutputStockLoc)
                .Include(_ => _.WhPackStockLoc)
                .Include(_ => _.WhQcStockLoc)
                .FirstOrDefault(_ => _.Id == id);
            if (stockWarehouse == null)
            {
                throw new Exception("Warehouse not exists");
            }
            result.Succeed = true;
            result.Data = _mapper.Map<StockWarehouseInfo>(stockWarehouse);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
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
                child.CompleteName = $"{child.ParentLocation.CompleteName}/{child.Name}";
                child.ParentPath = $"{child.ParentLocation.ParentPath}/{child.Id}";
            }

            UpdateCompleteNameAndParentPathRecursive(dbContext, child.Id);
        }
    }
}
