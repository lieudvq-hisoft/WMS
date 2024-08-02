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

public interface IStockQuantService
{
    Task<ResultModel> Create(StockQuantCreate model);
    Task<ResultModel> GetStockMoveLines(PagingParam<StockMoveLineSortCriteria> paginationModel, Guid id);
    Task<ResultModel> Update(StockQuantUpdate model);
    Task<ResultModel> SetStockQuant(Guid id);
    Task<ResultModel> ClearStockQuant(Guid id);
    Task<ResultModel> ApplyStockQuant(Guid id);

}
public class StockQuantService : IStockQuantService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly Guid _virtualLocationId = new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071");
    private readonly Guid _inventoryAdjustmentId = new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6");
    private readonly Guid _physicalLocationId = new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc");
    public StockQuantService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(StockQuantCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var product = _dbContext.ProductProduct.Include(_ => _.ProductTemplate).FirstOrDefault(_ => _.Id == model.ProductId);
                if(product == null)
                {
                    throw new Exception("Product not exists");

                }
                var stockQuant = _dbContext.StockQuant.FirstOrDefault(_ => _.ProductId == model.ProductId && _.LocationId == model.LocationId);
                if (stockQuant != null)
                {
                    throw new Exception("This record already exists");
                }
                var newStockQuant = _mapper.Map<StockQuantCreate, StockQuant>(model);

                if (newStockQuant.Quantity <= 0)
                {
                    throw new Exception("Stock quantity must be greater than 0");
                }
                _dbContext.StockQuant.Add(newStockQuant);

                var stockMove = new StockMove
                {
                    ProductId = product.Id,
                    ProductUomId = product.ProductTemplate.UomId,
                    LocationId = _inventoryAdjustmentId,
                    LocationDestId = newStockQuant.LocationId,
                    Name = "Product Quantity Updated",
                    State = StockMoveState.Done,
                    Reference = "Product Quantity Updated",
                    ProductQty = newStockQuant.Quantity,
                    ProductUomQty = newStockQuant.Quantity,
                    Quantity = newStockQuant.Quantity,
                };
                _dbContext.StockMove.Add(stockMove);

                var stockMoveLine = new StockMoveLine
                {
                    MoveId = stockMove.Id,
                    ProductUomId = product.ProductTemplate.UomId,
                    QuantId = newStockQuant.Id,
                    State = StockMoveState.Done,
                    QuantityProductUom = newStockQuant.Quantity,
                    Quantity = newStockQuant.Quantity,
                    LocationId = _inventoryAdjustmentId,
                    LocationDestId = newStockQuant.LocationId,
                };
                _dbContext.StockMoveLine.Add(stockMoveLine);
                
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = _mapper.Map<StockQuant, StockQuantModel>(newStockQuant);
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

    public async Task<ResultModel> GetStockMoveLines(PagingParam<StockMoveLineSortCriteria> paginationModel, Guid id)
    {
        var result = new ResultModel();
        try
        {
            var stockMoveLines = _dbContext.StockMoveLine
                .Include(_ => _.StockMove)
                    .ThenInclude(_ => _.ProductProduct)
                    .ThenInclude(_ => _.ProductTemplate)
                .Include(_ => _.StockMove)
                    .ThenInclude(_ => _.ProductProduct)
                    .ThenInclude(_ => _.ProductVariantCombinations)
                    .ThenInclude(_ => _.ProductTemplateAttributeValue)
                    .ThenInclude(_ => _.ProductAttributeValue)
                .Include(_ => _.ProductUom)
                .Where(_ => _.QuantId == id)
                .Include(_ => _.Location)
                .Include(_ => _.LocationDest)
                .AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockMoveLines.Count());
            stockMoveLines = stockMoveLines.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockMoveLines = stockMoveLines.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = stockMoveLines.Select(sml => new StockMoveLineView
            {
                Reference = sml.StockMove.Reference,
                ProductProduct = sml.StockMove.ProductProduct.ProductTemplate.Name + " (" +
                     string.Join(", ", sml.StockMove.ProductProduct.ProductVariantCombinations.Select(pvc => pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name))+ ")",
                UomUom = sml.ProductUom.Name,
                QuantityProductUom = sml.QuantityProductUom,
                Quantity = sml.Quantity,
                State = sml.State,
                Location = sml.Location.CompleteName,
                LocationDest = sml.LocationDest.CompleteName,
                WriteDate = sml.WriteDate,
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

    public async Task<ResultModel> Update(StockQuantUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockQuant = _dbContext.StockQuant.FirstOrDefault(_ => _.Id == model.Id);
            if (stockQuant == null)
            {
                throw new Exception("This record not existed");
            }

            if (model.InventoryQuantity != null)
            {
                stockQuant.InventoryQuantity = model.InventoryQuantity;
                stockQuant.InventoryDiffQuantity = model.InventoryQuantity - stockQuant.Quantity;
                stockQuant.InventoryQuantitySet = true;
            }
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockQuant, StockQuantModel>(stockQuant);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateInventoryQuantitySet(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockQuant = _dbContext.StockQuant.FirstOrDefault(_ => _.Id == id);
            if (stockQuant == null)
            {
                throw new Exception("This record not existed");
            }

            stockQuant.InventoryQuantitySet = true;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockQuant, StockQuantModel>(stockQuant);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> SetStockQuant(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockQuant = _dbContext.StockQuant.FirstOrDefault(_ => _.Id == id);
            if (stockQuant == null)
            {
                throw new Exception("This record not existed");
            }

            stockQuant.InventoryQuantitySet = true;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockQuant, StockQuantModel>(stockQuant);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> ClearStockQuant(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockQuant = _dbContext.StockQuant.FirstOrDefault(_ => _.Id == id);
            if (stockQuant == null)
            {
                throw new Exception("This record not existed");
            }

            stockQuant.InventoryQuantitySet = false;
            stockQuant.InventoryQuantity = 0;
            stockQuant.InventoryDiffQuantity = 0;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockQuant, StockQuantModel>(stockQuant);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> ApplyStockQuant(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockQuant = _dbContext.StockQuant
                .Include(_ => _.ProductProduct).ThenInclude(_ => _.ProductTemplate)
                .FirstOrDefault(_ => _.Id == id);
            if (stockQuant == null)
            {
                throw new Exception("This record not existed");
            }

            if (stockQuant.InventoryQuantitySet == false)
            {
                throw new Exception("This record needs to be set");
            }

            stockQuant.Quantity = (decimal)(stockQuant.Quantity + stockQuant.InventoryDiffQuantity);
            stockQuant.InventoryQuantity = 0;

            var stockMove = new StockMove
            {
                ProductId = stockQuant.ProductProduct.Id,
                ProductUomId = stockQuant.ProductProduct.ProductTemplate.UomId,
                LocationId = stockQuant.InventoryDiffQuantity > 0 ? _inventoryAdjustmentId : stockQuant.LocationId,
                LocationDestId = stockQuant.InventoryDiffQuantity > 0 ? stockQuant.LocationId : _inventoryAdjustmentId,
                Name = "Product Quantity Updated",
                State = StockMoveState.Done,
                Reference = "Product Quantity Updated",
                ProductQty = (decimal)stockQuant.InventoryDiffQuantity,
                ProductUomQty = (decimal)stockQuant.InventoryDiffQuantity,
                Quantity = stockQuant.InventoryDiffQuantity,
            };
            _dbContext.StockMove.Add(stockMove);

            var stockMoveLine = new StockMoveLine
            {
                MoveId = stockMove.Id,
                ProductUomId = stockQuant.ProductProduct.ProductTemplate.UomId,
                QuantId = stockQuant.Id,
                State = StockMoveState.Done,
                QuantityProductUom = stockQuant.InventoryDiffQuantity,
                Quantity = (decimal)stockQuant.InventoryDiffQuantity,
                LocationId = stockQuant.InventoryDiffQuantity > 0 ? _inventoryAdjustmentId : stockQuant.LocationId,
                LocationDestId = stockQuant.InventoryDiffQuantity > 0 ? stockQuant.LocationId : _inventoryAdjustmentId,
            };
            _dbContext.StockMoveLine.Add(stockMoveLine);

            stockQuant.InventoryDiffQuantity = 0;
            stockQuant.InventoryQuantitySet = false;
            stockQuant.WriteDate = DateTime.Now;

            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockQuant, StockQuantModel>(stockQuant);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }


}
