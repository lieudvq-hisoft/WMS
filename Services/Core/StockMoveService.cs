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
using static QRCoder.PayloadGenerator;

namespace Services.Core;

public interface IStockMoveService
{
    Task<ResultModel> Create(StockMoveCreate model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> UpdateQuantity(StockMoveQuantityUpdate model);

}
public class StockMoveService : IStockMoveService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    private readonly Guid _virtualLocationId = new Guid("b7d84e2e-39f3-4a8e-a5a5-8b8e839e7071");
    private readonly Guid _inventoryAdjustmentId = new Guid("d95a2d57-68a6-4f85-b6b3-d3eb2a5b73a6");
    private readonly Guid _physicalLocationId = new Guid("e2a7c3e0-1a4d-43b6-95e1-123456789abc");
    public StockMoveService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(StockMoveCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var product = _dbContext.ProductProduct
                    .Include(_ => _.ProductVariantCombinations)
                        .ThenInclude(_ => _.ProductTemplateAttributeValue)
                        .ThenInclude(_ => _.ProductAttributeValue)
                    .Include(_ => _.ProductTemplate).FirstOrDefault(_ => _.Id == model.ProductId);
                if(product == null)
                {
                    throw new Exception("Product not exists");
                }
                var uomUom = _dbContext.UomUom.FirstOrDefault(_ => _.Id == model.ProductUomId);
                if(uomUom == null)
                {
                    throw new Exception("UomUom not exists");
                }
                var stockPicking = _dbContext.StockPicking.FirstOrDefault(_ => _.Id == model.PickingId);
                if (stockPicking == null)
                {
                    throw new Exception("Stock Picking not exists");
                }

                if (stockPicking.State == PickingState.Done)
                {
                    throw new Exception("You cannot create a stock move that has been set to 'Done'.");
                }

                if (stockPicking.State == PickingState.Cancelled)
                {
                    throw new Exception("You cannot create a stock move that has been set to 'Cancelled'.");
                }
                var stockMove = _mapper.Map<StockMoveCreate, StockMove>(model);
                stockMove.Name = product.ProductTemplate.Name + " (" + string.Join(", ", product.ProductVariantCombinations.Select(pvc => pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name)) + ")";
                stockMove.Reference = stockPicking.Name;
                //decimal quantity =  stockMove.ProductUomQty / uomUom.Factor;
                //quantity = Math.Round(quantity / uomUom.Rounding) * uomUom.Rounding;
                //stockMove.Quantity = quantity;
                if (stockPicking.State == PickingState.Assigned)
                {
                    stockMove.State = StockMoveState.Assigned;
                }
                _dbContext.Add(stockMove);
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = stockMove.Id;
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

    public async Task<ResultModel> Delete(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var stockMove = _dbContext.StockMove.Include(_ => _.StockPicking).FirstOrDefault(_ => _.Id == id);
                if (stockMove == null)
                {
                    throw new Exception("Stock Mone not exists");
                }
                if (stockMove.State == StockMoveState.Done)
                {
                    throw new Exception("You cannot delete a stock move that has been set to 'Done'.");

                }
                _dbContext.Remove(stockMove);
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = stockMove.Id;
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

    public async Task<ResultModel> UpdateQuantity(StockMoveQuantityUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var stockMove = _dbContext.StockMove.FirstOrDefault(_ => _.Id == model.Id);
                if (stockMove == null)
                {
                    throw new Exception("Stock Mone not exists");
                }
                if (stockMove.State == StockMoveState.Done)
                {
                    throw new Exception("You cannot update a stock move that has been set to 'Done'.");

                }
                stockMove.Quantity = model.Quantity > 0 ? model.Quantity : 0;
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = stockMove.Id;
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
