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
using Services.Utils;

namespace Services.Core;

public interface IStockLotService
{
    Task<ResultModel> Create(StockLotCreate model);
    Task<ResultModel> Update(StockLotUpdate model);
    Task<ResultModel> Get(PagingParam<StockLotSortCriteria> paginationModel);
    Task<ResultModel> Delete(Guid id);
}
public class StockLotService : IStockLotService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public StockLotService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(StockLotCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockLot = _mapper.Map<StockLotCreate, StockLot>(model);
            _dbContext.Add(stockLot);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockLot, StockLotModel>(stockLot);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(StockLotUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var stockLot = _dbContext.StockLot.FirstOrDefault(_ => _.Id == model.Id);
            if (stockLot == null)
            {
                throw new Exception("Stock Lot not exists");
            }
            if (model.ProductId != null)
            {
                stockLot.ProductId = (Guid)model.ProductId;
            }
            if (model.Name != null)
            {
                stockLot.Name = model.Name;
            }
            if (model.Note != null)
            {
                stockLot.Note = model.Note;
            }
            stockLot.WriteDate = DateTime.Now;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<StockLot, StockLotModel>(stockLot);

        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Get(PagingParam<StockLotSortCriteria> paginationModel)
    {
        var result = new ResultModel();
        try
        {
            var stockLots = _dbContext.StockLot
                .Include(_ => _.ProductProduct)
                .AsQueryable();
            if (!string.IsNullOrEmpty(paginationModel.SearchText))
            {
                stockLots = stockLots.Where(_ => _.Name.Contains(paginationModel.SearchText));
            }
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockLots.Count());
            stockLots = stockLots.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockLots = stockLots.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<StockLotModel>(stockLots);
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
            var stockLot = _dbContext.StockLot.FirstOrDefault(_ => _.Id == id);
            if (stockLot == null)
            {
                throw new Exception("Stock Lot not exists");
            }
            _dbContext.Remove(stockLot);
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

}
