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
using static Confluent.Kafka.ConfigPropertyNames;

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
    Task<ResultModel> CreateReceipt(StockPickingReceipt model, Guid createId);
    Task<ResultModel> UpdateReceipt(StockPickingUpdateReceipt model);
    Task<ResultModel> GetInfo(Guid id);
    Task<ResultModel> GetStockMoves(PagingParam<StockMoveSortCriteria> paginationModel, Guid id);
    Task<ResultModel> MakeAsTodo(Guid id);
    Task<ResultModel> Cancel(Guid id);
    Task<ResultModel> Validate(Guid id);
}
public class StockPickingService : IStockPickingService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly Guid _partnerLocationId = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479");
    private readonly Guid _vendorLocationId = new Guid("6ba7b810-9dad-11d1-80b4-00c04fd430c8");
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
            var stockPicking = _dbContext.StockPicking.Include(_ => _.StockMoves).FirstOrDefault(_ => _.Id == model.Id);
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
                foreach (var stockMove in stockPicking.StockMoves)
                {
                    stockMove.LocationDestId = (Guid)model.LocationDestId;
                }
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

    public async Task<ResultModel> CreateReceipt(StockPickingReceipt model, Guid createUid)
    {
        var result = new ResultModel();
        try
        {
            var stockPicking = _mapper.Map<StockPickingReceipt, StockPicking>(model);
            if(stockPicking.PartnerId == null)
            {
                stockPicking.LocationId = _vendorLocationId;
            }
            var stockPickingType = _dbContext.StockPickingType.FirstOrDefault(_ => _.Id == model.PickingTypeId);
            if(stockPickingType == null)
            {
                throw new Exception("Stock Picking Type not exists");
            }
            if(stockPickingType.Code != StockPickingTypeCode.Incoming)
            {
                throw new Exception("Stock Picking Type not used for receipt");
            }
            stockPicking.Name = $"{stockPickingType.Barcode}";
            stockPicking.CreateUid = createUid;
            _dbContext.Add(stockPicking);
            stockPicking.Name = $"{stockPickingType.Barcode}-{stockPicking.Id}";
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

    public async Task<ResultModel> GetInfo(Guid id)
    {
        var result = new ResultModel();
        try
        {
            var stockPicking = _dbContext.StockPicking
                .Include(_ => _.Location)
                .Include(_ => _.LocationDest)
                .Include(_ => _.PickingType)
                .Include(_ => _.StockMoves)
                .FirstOrDefault(_ => _.Id == id);
            if (stockPicking == null)
            {
                throw new Exception("Stock Picking not exists");
            }
            if(stockPicking.StockMoves.Count() == 0)
            {
                stockPicking.State = PickingState.Draft;
                _dbContext.SaveChanges();
            }
            result.Succeed = true;
            result.Data = _mapper.Map<StockPicking, StockPickingModel>(stockPicking); ;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateReceipt(StockPickingUpdateReceipt model)
    {
        var result = new ResultModel();
        try
        {
            var stockPicking = _dbContext.StockPicking.Include(_ => _.StockMoves).FirstOrDefault(_ => _.Id == model.Id);
            if (stockPicking == null)
            {
                throw new Exception("Stock Picking not exists");
            }
            if (stockPicking.State == PickingState.Done)
            {
                throw new Exception("You cannot update a stock picking that has been set to 'Done'.");

            }
            if (stockPicking.State == PickingState.Cancelled)
            {
                throw new Exception("You cannot update a stock picking that has been set to 'Cancelled'.");

            }
            if (model.LocationDestId != null)
            {
                stockPicking.LocationDestId = (Guid)model.LocationDestId;
                if (stockPicking.StockMoves.Any())
                {
                    foreach (var stockMove in stockPicking.StockMoves)
                    {
                        stockMove.LocationDestId = stockPicking.LocationDestId;
                    }
                }
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

    public async Task<ResultModel> GetStockMoves(PagingParam<StockMoveSortCriteria> paginationModel, Guid id)
    {
        var result = new ResultModel();
        try
        {
            var stockMoves = _dbContext.StockMove
                .Include(_ => _.ProductProduct)
                    .ThenInclude(_ => _.ProductTemplate)
                .Include(_ => _.ProductProduct)
                    .ThenInclude(_ => _.ProductVariantCombinations)
                    .ThenInclude(_ => _.ProductTemplateAttributeValue)
                    .ThenInclude(_ => _.ProductAttributeValue)
                .Include(_ => _.ProductUom)
                .Where(_ => _.PickingId == id)
                .Include(_ => _.Location)
                .Include(_ => _.LocationDest)
                .AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockMoves.Count());
            stockMoves = stockMoves.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockMoves = stockMoves.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = stockMoves.Select(sm => new StockMoveModel
            {
                Id = sm.Id,
                ProductProduct = new ProductProductModel
                {
                    Id = sm.ProductProduct.Id,
                    Name = sm.ProductProduct.ProductTemplate.Name,
                    Pvcs = sm.ProductProduct.ProductVariantCombinations.Select(pvc =>
                    new Pvc
                    {
                        Attribute = pvc.ProductTemplateAttributeValue.ProductAttributeValue.ProductAttribute.Name,
                        Value = pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name
                    })
                    .ToList(),
                    QtyAvailable = sm.ProductProduct.StockQuants.Sum(sq => sq.Quantity),
                },
                UomUom = _mapper.Map<UomUom, UomUomModel>(sm.ProductUom),
                Location = _mapper.Map<StockLocation, StockLocationModel>(sm.Location),
                LocationDest = _mapper.Map<StockLocation, StockLocationModel>(sm.LocationDest),
                PickingId = sm.PickingId,
                Name = sm.Name,
                State = sm.State,
                Reference = sm.Reference,
                DescriptionPicking = sm.DescriptionPicking,
                ProductQty = sm.ProductQty,
                ProductUomQty = sm.ProductUomQty,
                Quantity = sm.Quantity,
                ReservationDate = sm.ReservationDate,
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

    public async Task<ResultModel> MakeAsTodo(Guid id)
    {
        var result = new ResultModel();
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var stockPicking = _dbContext.StockPicking.Include(_ => _.StockMoves).FirstOrDefault(_ => _.Id == id);
                if (stockPicking == null)
                {
                    throw new Exception("Stock Picking not exists");
                }
                if (stockPicking.State == PickingState.Done)
                {
                    throw new Exception("You cannot update a stock picking that has been set to 'Done'.");

                }

                if (stockPicking.State == PickingState.Cancelled)
                {
                    throw new Exception("You cannot update a stock picking that has been set to 'Cancelled'.");

                }

                if (stockPicking.StockMoves.Count() == 0)
                {
                    throw new Exception("There are no operations for make as to do.");
                }

                foreach (var stockMove in stockPicking.StockMoves)
                {
                    stockMove.Quantity = stockMove.ProductUomQty;
                    stockMove.State = StockMoveState.Assigned;
                }
                stockPicking.State = PickingState.Assigned;
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = stockPicking.Id;
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

    public async Task<ResultModel> Cancel(Guid id)
    {
        var result = new ResultModel();
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var stockPicking = _dbContext.StockPicking.Include(_ => _.StockMoves).FirstOrDefault(_ => _.Id == id);
                if (stockPicking == null)
                {
                    throw new Exception("Stock Picking not exists");
                }
                if (stockPicking.State == PickingState.Done)
                {
                    throw new Exception("You cannot update a stock picking that has been set to 'Done'.");

                }

                if (stockPicking.State == PickingState.Cancelled)
                {
                    throw new Exception("You cannot update a stock picking that has been set to 'Cancelled'.");

                }

                foreach (var stockMove in stockPicking.StockMoves)
                {
                    stockMove.State = StockMoveState.Cancelled;
                }
                stockPicking.State = PickingState.Cancelled;
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = stockPicking.Id;
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

    public async Task<ResultModel> Validate(Guid id)
    {
        var result = new ResultModel();
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var stockPicking = _dbContext.StockPicking
                    .Include(_ => _.StockMoves)
                        .ThenInclude(_ => _.ProductUom)
                    .FirstOrDefault(_ => _.Id == id);
                if (stockPicking == null)
                {
                    throw new Exception("Stock Picking not exists");
                }
                if (stockPicking.State == PickingState.Done)
                {
                    throw new Exception("You cannot update a stock picking that has been set to 'Done'.");

                }

                if (stockPicking.State == PickingState.Cancelled)
                {
                    throw new Exception("You cannot update a stock picking that has been set to 'Cancelled'.");

                }

                foreach (var stockMove in stockPicking.StockMoves)
                {
                    if(stockMove.ProductUomQty > stockMove.Quantity)
                    {
                        throw new Exception("You have processed less products than the initial demand.");
                    }

                    decimal quantity = (decimal)(stockMove.Quantity / stockMove.ProductUom.Factor);
                    quantity = Math.Round(quantity / stockMove.ProductUom.Rounding) * stockMove.ProductUom.Rounding;
                    stockMove.ProductQty = quantity;
                    stockMove.State = StockMoveState.Done;
                    var stockQuant = _dbContext.StockQuant.FirstOrDefault(_ => _.LocationId == stockMove.LocationDestId && _.ProductId == stockMove.ProductId);

                    if(stockQuant == null)
                    {
                        stockQuant = new StockQuant
                        {
                            ProductId = stockMove.ProductId,
                            LocationId = stockMove.LocationDestId,
                            Quantity = (decimal)stockMove.ProductQty,
                        };
                        _dbContext.Add(stockQuant);
                    }else
                    {
                        stockQuant.Quantity = (decimal)(stockQuant.Quantity + stockMove.ProductQty);
                    }

                    var stockMoveLine = new StockMoveLine
                    {
                        MoveId = stockMove.Id,
                        ProductUomId = stockMove.ProductUomId,
                        QuantId = stockQuant.Id,
                        State = StockMoveState.Done,
                        QuantityProductUom = stockMove.Quantity,
                        Quantity = (decimal)stockMove.ProductQty,
                        LocationId = stockMove.LocationId,
                        LocationDestId = stockMove.LocationDestId,
                    };
                    _dbContext.StockMoveLine.Add(stockMoveLine);

                }
                stockPicking.State = PickingState.Done;
                stockPicking.DateDone = DateTime.Now;
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = stockPicking.Id;
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
}
