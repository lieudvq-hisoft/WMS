using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Services.Core;

public interface IUomCategoryService
{
    Task<ResultModel> Get();
    Task<ResultModel> Create(UomCategoryCreate model);
    Task<ResultModel> GetUomUom(Guid uomCateId);

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

    public async Task<ResultModel> Get()
    {
        var result = new ResultModel();
        try
        {
            var uomCategories = _dbContext.UomCategory.AsQueryable();
            var data = _mapper.ProjectTo<UomCategoryModel>(uomCategories);
            result.Succeed = true;
            result.Data = data;
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

    public async Task<ResultModel> GetUomUom(Guid uomCateId)
    {
        var result = new ResultModel();
        try
        {
            var uomUoms = _dbContext.UomUom.Where(_ => _.CategoryId == uomCateId).AsQueryable();
            var data = _mapper.ProjectTo<UomUomCollection>(uomUoms);
            result.Succeed = true;
            result.Data = data;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
