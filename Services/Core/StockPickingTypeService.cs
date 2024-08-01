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

public interface IStockPickingTypeService
{
    Task<ResultModel> Get(PagingParam<SortStockPickingTypeCriteria> paginationModel);
}
public class StockPickingTypeService : IStockPickingTypeService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;
    public StockPickingTypeService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Get(PagingParam<SortStockPickingTypeCriteria> paginationModel)
    {
        var result = new ResultModel();
        try
        {
            var stockPickingTypes = _dbContext.StockPickingType
                .Include(_ => _.StockPickings)
                .Include(_ => _.Warehouse).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockPickingTypes.Count());
            stockPickingTypes = stockPickingTypes.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockPickingTypes = stockPickingTypes.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<StockPickingTypeInfo>(stockPickingTypes);
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
