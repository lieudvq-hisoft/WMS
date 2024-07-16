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

public interface IProductRemovalService
{
    Task<ResultModel> Create(ProductRemovalCreate model);
    Task<ResultModel> Update(ProductRemovalUpdate model);
    Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel);
    Task<ResultModel> Delete(Guid id);
}
public class ProductRemovalService : IProductRemovalService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ProductRemovalService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(ProductRemovalCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productRemoval = _mapper.Map<ProductRemovalCreate, ProductRemoval>(model);
            _dbContext.Add(productRemoval);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductRemoval, ProductRemovalModel>(productRemoval);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(ProductRemovalUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productRemoval = _dbContext.ProductRemoval.FirstOrDefault(_ => _.Id == model.Id);
            if (productRemoval == null)
            {
                throw new Exception("Product Removal not exists");
            }
            if(model.Name != null)
            {
                productRemoval.Name = model.Name;
            }
            if(model.Method != null)
            {
                productRemoval.Method = model.Method.ToString();
            }
            productRemoval.WriteDate = DateTime.Now;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductRemoval, ProductRemovalModel>(productRemoval);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel)
    {
        var result = new ResultModel();
        try
        {
            var productRemovals = _dbContext.ProductRemoval.AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, productRemovals.Count());
            productRemovals = productRemovals.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            productRemovals = productRemovals.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ProductRemovalModel>(productRemovals);
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
            var productRemoval = _dbContext.ProductRemoval.FirstOrDefault(_ => _.Id == id);
            if (productRemoval == null)
            {
                throw new Exception("Product Removal not exists");
            }
            _dbContext.Remove(productRemoval);
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
