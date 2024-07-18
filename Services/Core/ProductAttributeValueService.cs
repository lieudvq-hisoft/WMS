using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Services.Core;

public interface IProductAttributeValueService
{
    Task<ResultModel> Create(ProductAttributeValueCreate model);
    Task<ResultModel> Update(ProductAttributeValueUpdate model);
    Task<ResultModel> Delete(Guid id);
}
public class ProductAttributeValueService : IProductAttributeValueService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ProductAttributeValueService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(ProductAttributeValueCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {

            var productAttribute = _mapper.Map<ProductAttributeValueCreate, ProductAttributeValue>(model);
            _dbContext.Add(productAttribute);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductAttributeValue, ProductAttributeValueModel>(productAttribute);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(ProductAttributeValueUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productAttributeValue = _dbContext.ProductAttributeValue.FirstOrDefault(_ => _.Id == model.Id);
            if (productAttributeValue == null)
            {
                throw new Exception("Attribute value not exists");
            }
            if (model.Name != null)
            {
                productAttributeValue.Name = model.Name;
            }
            productAttributeValue.WriteDate = DateTime.Now;

            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductAttributeValue, ProductAttributeValueModel>(productAttributeValue);

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
            var productAttributeValue = _dbContext.ProductAttributeValue.FirstOrDefault(_ => _.Id == id);
            if (productAttributeValue == null)
            {
                throw new Exception("Attribute value not exists");
            }
            _dbContext.Remove(productAttributeValue);
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
