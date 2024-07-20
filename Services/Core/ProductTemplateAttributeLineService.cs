using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Services.Core;

public interface IProductTemplateAttributeLineService
{
    Task<ResultModel> Create(ProductTemplateAttributeLineCreate model);
    Task<ResultModel> Update(ProductTemplateAttributeLineUpdate model);
    Task<ResultModel> Delete(Guid id);
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

}
