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

public interface IProductTemplateService
{
    Task<ResultModel> Create(ProductTemplateCreate model);
    Task<ResultModel> Update(ProductTemplateUpdate model);
    Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetInfo(Guid id);
    Task<ResultModel> GetAttributeLine(PagingParam<ProductTemplateAttributeLineSortCriteria> paginationModel, Guid id);
}
public class ProductTemplateService : IProductTemplateService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ProductTemplateService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(ProductTemplateCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productTemplate = _mapper.Map<ProductTemplateCreate, ProductTemplate>(model);
            _dbContext.Add(productTemplate);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductTemplate, ProductTemplateModel>(productTemplate);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(ProductTemplateUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productTemplate = _dbContext.ProductTemplate.FirstOrDefault(_ => _.Id == model.Id);
            if (productTemplate == null)
            {
                throw new Exception("Product Template not exists");
            }
            if (model.CategId != null)
            {
                productTemplate.CategId = (Guid)model.CategId;
            }
            if (model.UomId != null)
            {
                productTemplate.UomId = (Guid)model.UomId;
            }
            if (model.Description != null)
            {
                productTemplate.Description = model.Description;
            }
            if (model.Tracking != null)
            {
                productTemplate.Tracking = model.Tracking;
            }
            if (model.DetailedType != null)
            {
                productTemplate.DetailedType = model.DetailedType;
            }
            if (model.Name != null)
            {
                productTemplate.Name = model.Name;
            }
            productTemplate.WriteDate = DateTime.Now;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductTemplate, ProductTemplateModel>(productTemplate);

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
            var productTemplates = _dbContext.ProductTemplate.AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, productTemplates.Count());
            productTemplates = productTemplates.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            productTemplates = productTemplates.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ProductTemplateModel>(productTemplates);
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
            var productTemplate = _dbContext.ProductTemplate.FirstOrDefault(_ => _.Id == id);
            if (productTemplate == null)
            {
                throw new Exception("Product Template not exists");
            }
            _dbContext.Remove(productTemplate);
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
            var productTemplate = _dbContext.ProductTemplate.Include(_ => _.ProductCategory).Include(_ => _.UomUom).FirstOrDefault(_ => _.Id == id);
            if (productTemplate == null)
            {
                throw new Exception("Product Category not exists");
            }
            result.Succeed = true;
            result.Data = _mapper.Map<ProductTemplateInfo>(productTemplate);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetAttributeLine(PagingParam<ProductTemplateAttributeLineSortCriteria> paginationModel, Guid id)
    {
        var result = new ResultModel();
        try
        {
            var productTemplateAttributeLines = _dbContext.ProductTemplateAttributeLine
                .Include(_ => _.ProductAttribute)
                .Include(_ => _.ProductTemplateAttributeValues).ThenInclude(_ => _.ProductAttributeValue)
                .Where(_ => _.ProductTmplId == id).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, productTemplateAttributeLines.Count());
            productTemplateAttributeLines = productTemplateAttributeLines.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            productTemplateAttributeLines = productTemplateAttributeLines.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ProductTemplateAttributeLineInfo>(productTemplateAttributeLines);
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
