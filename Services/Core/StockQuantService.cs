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

public interface IStockQuantService
{
    Task<ResultModel> Create(StockQuantCreate model);
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
}
