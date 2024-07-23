using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Asn1.Ocsp;

namespace Services.Core;

public interface IProductTemplateAttributeLineService
{
    Task<ResultModel> Create(ProductTemplateAttributeLineCreate model);
    Task<ResultModel> Update(ProductTemplateAttributeLineUpdate model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> UpdateAttributeValues(ProductTemplateAttributeValueCreate model);
}
public class ProductTemplateAttributeLineService : IProductTemplateAttributeLineService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ProductTemplateAttributeLineService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(ProductTemplateAttributeLineCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var ptal = _dbContext.ProductTemplateAttributeLine.FirstOrDefault(_ => _.AttributeId == model.AttributeId && _.ProductTmplId == model.ProductTmplId);
            if (ptal != null)
            {
                throw new Exception("Product Template Attribute Line existed");
            }
            var productTemplateAttributeLine = _mapper.Map<ProductTemplateAttributeLineCreate, ProductTemplateAttributeLine>(model);
            _dbContext.Add(productTemplateAttributeLine);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductTemplateAttributeLine, ProductTemplateAttributeLineModel>(productTemplateAttributeLine);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(ProductTemplateAttributeLineUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productTemplateAttributeLine = _dbContext.ProductTemplateAttributeLine.FirstOrDefault(_ => _.Id == model.Id);
            if (productTemplateAttributeLine == null)
            {
                throw new Exception("Product Template Attribute Line not exists");
            }
            if (model.Active != null)
            {
                productTemplateAttributeLine.Active = model.Active;
            }
            productTemplateAttributeLine.WriteDate = DateTime.Now;

            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductTemplateAttributeLine, ProductTemplateAttributeLineModel>(productTemplateAttributeLine);

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
            var productTemplateAttributeLine = _dbContext.ProductTemplateAttributeLine.FirstOrDefault(_ => _.Id == id);
            if (productTemplateAttributeLine == null)
            {
                throw new Exception("Product Template Attribute Line not exists");
            }
            _dbContext.Remove(productTemplateAttributeLine);
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

    public async Task<ResultModel> UpdateAttributeValues(ProductTemplateAttributeValueCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var ptal = _dbContext.ProductTemplateAttributeLine
                .Include(_ => _.ProductTemplateAttributeValues)
                .FirstOrDefault(_ => _.Id == model.AttributeLineId);
            if (ptal == null)
            {
                throw new Exception("Product Template Attribute Line note exists");
            }
            //var existingIds = ptal.ProductTemplateAttributeValues.Select(x => x.Id).ToList();
            var existingIds = ptal.ProductTemplateAttributeValues.Select(_ => _.ProductAttributeValueId).ToList();
            var idsToAdd = model.ProductAttributeValueIds.Except(existingIds).ToList();
            var idsToRemove = existingIds.Except(model.ProductAttributeValueIds).ToList();

            foreach (var id in idsToAdd)
            {
                var newValue = new ProductTemplateAttributeValue
                {
                    AttributeLineId = model.AttributeLineId,
                    ProductAttributeValueId = id,
                };
                _dbContext.ProductTemplateAttributeValue.Add(newValue);
            }

            foreach (var id in idsToRemove)
            {
                var valueToRemove = ptal.ProductTemplateAttributeValues.FirstOrDefault(x => x.Id == id);
                if (valueToRemove != null)
                {
                    _dbContext.ProductTemplateAttributeValue.Remove(valueToRemove);
                }
            }
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = "";
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }


}
