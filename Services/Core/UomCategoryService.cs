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

public interface IUomCategoryService
{
    Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel);
    Task<ResultModel> Create(UomCategoryCreate model);
    Task<ResultModel> GetUomUom(PagingParam<SortUomUomCriteria> paginationModel, Guid uomCateId);
    Task<ResultModel> UpdateInfo(UomCategoryUpdate model);
    Task<ResultModel> GetInfo(Guid id);
    Task<ResultModel> Delete(Guid id);
}
public class UomCategoryService : IUomCategoryService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UomCategoryService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel)
    {
        var result = new ResultModel();
        try
        {
            var uomCategories = _dbContext.UomCategory.Include(_ => _.UomUoms.OrderByDescending(u => u.CreateDate)).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, uomCategories.Count());
            uomCategories = uomCategories.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            uomCategories = uomCategories.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<UomCategoryModel>(uomCategories);
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

    public async Task<ResultModel> Create(UomCategoryCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var uomCategory = _mapper.Map<UomCategoryCreate, UomCategory>(model);
            _dbContext.UomCategory.Add(uomCategory);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = uomCategory.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetUomUom(PagingParam<SortUomUomCriteria> paginationModel, Guid uomCateId)
    {
        var result = new ResultModel();
        try
        {
            var uomUoms = _dbContext.UomUom.Where(_ => _.CategoryId == uomCateId).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, uomUoms.Count());
            uomUoms = uomUoms.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            uomUoms = uomUoms.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<UomUomCollection>(uomUoms);
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

    public async Task<ResultModel> UpdateInfo(UomCategoryUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var uomCate = _dbContext.UomCategory.FirstOrDefault(_ => _.Id == model.Id);
            if (uomCate == null)
            {
                throw new Exception("Uom Category not exists");
            }
            uomCate.Name = model.Name;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<UomCategoryModel>(uomCate);
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
            var uomCate = _dbContext.UomCategory.FirstOrDefault(_ => _.Id == id);
            if (uomCate == null)
            {
                throw new Exception("Uom Category not exists");
            }
            result.Succeed = true;
            result.Data = _mapper.Map<UomCategoryInfo>(uomCate);
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
            var uomCategory = _dbContext.UomCategory.FirstOrDefault(_ => _.Id == id);
            if (uomCategory == null)
            {
                throw new Exception("Uom Category not exists");
            }
            _dbContext.UomCategory.Remove(uomCategory);
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
