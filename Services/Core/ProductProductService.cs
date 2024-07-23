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

public interface IProductProductService
{
    Task<ResultModel> Delete(Guid id);
}
public class ProductProductService : IProductProductService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ProductProductService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Delete(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productProduct = _dbContext.ProductProduct.Include(_ => _.ProductVariantCombinations).FirstOrDefault(_ => _.Id == id);
            if (productProduct == null)
            {
                throw new Exception("Product Variant not exists");
            }
            _dbContext.ProductVariantCombination.RemoveRange(productProduct.ProductVariantCombinations);
            _dbContext.Remove(productProduct);
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
