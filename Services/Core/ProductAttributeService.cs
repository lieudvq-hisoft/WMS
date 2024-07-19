using System.Xml.Linq;
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
using Org.BouncyCastle.Asn1.Ocsp;

namespace Services.Core;

public interface IProductAttributeService
{
    Task<ResultModel> Create(ProductAttributeCreate model);
    Task<ResultModel> Update(ProductAttributeUpdate model);
    Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetAttributeValue(PagingParam<SortCriteria> paginationModel, Guid id);
}
public class ProductAttributeService : IProductAttributeService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ProductAttributeService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(ProductAttributeCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {

            var productAttribute = _mapper.Map<ProductAttributeCreate, ProductAttribute>(model);
            _dbContext.Add(productAttribute);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductAttribute, ProductAttributeModel>(productAttribute);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(ProductAttributeUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productAttribute = _dbContext.ProductAttribute.FirstOrDefault(_ => _.Id == model.Id);
            if (productAttribute == null)
            {
                throw new Exception("Attribute not exists");
            }
            if (model.Name != null)
            {
                productAttribute.Name = model.Name;
            }
            productAttribute.WriteDate = DateTime.Now;

            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductAttribute, ProductAttributeModel>(productAttribute);

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
            var productAttributes = _dbContext.ProductAttribute.Include(_ => _.ProductAttributeValues).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, productAttributes.Count());
            productAttributes = productAttributes.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            productAttributes = productAttributes.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ProductAttributeModel>(productAttributes);
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

    public async Task<ResultModel> GetAttributeValue(PagingParam<SortCriteria> paginationModel, Guid id)
    {
        var result = new ResultModel();
        try
        {
            var productAttribute = _dbContext.ProductAttribute.FirstOrDefault(_ => _.Id == id);
            if (productAttribute == null)
            {
                throw new Exception("Attribute not exists");
            }

            var attributeValues = _dbContext.ProductAttributeValue.Where(_ => _.AttributeId == productAttribute.Id).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, attributeValues.Count());
            attributeValues = attributeValues.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            attributeValues = attributeValues.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ProductAttributeValueModel>(attributeValues);
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
            var productAttribute = _dbContext.ProductAttribute.FirstOrDefault(_ => _.Id == id);
            if (productAttribute == null)
            {
                throw new Exception("Attribute not exists");
            }
            _dbContext.Remove(productAttribute);
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
