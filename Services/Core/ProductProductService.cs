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
    Task<ResultModel> GetStockQuant(PagingParam<StockQuantSortCriteria> paginationModel, Guid id);
    Task<ResultModel> GetProductVariant();
    Task<ResultModel> GetUomUomForSelect(Guid id);
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

    public async Task<ResultModel> GetProductVariant()
    {
        var result = new ResultModel();
        try
        {
            var productProducts = _dbContext.ProductProduct
                .Include(_ => _.ProductTemplate)
                .Include(_ => _.ProductVariantCombinations)
                .ThenInclude(_ => _.ProductTemplateAttributeValue)
                .ThenInclude(_ => _.ProductAttributeValue)
                .ThenInclude(_ => _.ProductAttribute)
                .Include(_ => _.StockQuants).AsQueryable();
            //var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, productProducts.Count());
            //productProducts = productProducts.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            //productProducts = productProducts.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = productProducts
                .Select(_ =>
                new ProductProductModel
                {
                    Id = _.Id,
                    Name = _.ProductTemplate.Name,
                    Pvcs = _.ProductVariantCombinations.Select(pvc =>
                    new Pvc
                    {
                        Attribute = pvc.ProductTemplateAttributeValue.ProductAttributeValue.ProductAttribute.Name,
                        Value = pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name
                    })
                    .ToList(),
                    QtyAvailable = _.StockQuants.Sum(sq => sq.Quantity),
                    UomUom = _.ProductTemplate.UomUom.Name
                });
            //paging.Data = viewModels;
            result.Succeed = true;
            //result.Data = paging;
            result.Data = viewModels;
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

    public async Task<ResultModel> GetStockQuant(PagingParam<StockQuantSortCriteria> paginationModel, Guid id)
    {
        var result = new ResultModel();
        try
        {
            var stockQuants = _dbContext.StockQuant
                .Include(sq => sq.StockLocation)
                .Include(sq => sq.ProductProduct)
                    .ThenInclude(pp => pp.ProductTemplate)
                    .ThenInclude(pt => pt.UomUom)
                 .Include(sq => sq.ProductProduct)
                    .ThenInclude(pp => pp.ProductVariantCombinations)
                .Where(sq => sq.ProductId == id && sq.StockLocation.Usage == LocationType.Internal)
                .AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, stockQuants.Count());
            stockQuants = stockQuants.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            stockQuants = stockQuants.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = stockQuants
                .Select(_ => new StockQuantInfo
                {
                    Id = _.Id,
                    ProductProduct = new ProductProductModel
                    {
                        Id = _.ProductId,
                        Name = _.ProductProduct.ProductTemplate.Name,
                        Pvcs = _.ProductProduct.ProductVariantCombinations.Select(pvc =>
                            new Pvc
                            {
                                Attribute = pvc.ProductTemplateAttributeValue.ProductAttributeValue.ProductAttribute.Name,
                                Value = pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name
                            })
                            .ToList()
                    },
                    StockLocation = _mapper.Map<StockLocationModel>(_.StockLocation),
                    InventoryDate = _.InventoryDate,
                    Quantity = _.Quantity,
                    UomUom = _.ProductProduct.ProductTemplate.UomUom.Name,
                    InventoryQuantity = _.InventoryQuantity,
                    InventoryDiffQuantity = _.InventoryDiffQuantity,
                    InventoryQuantitySet = _.InventoryQuantitySet,
                });
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

    public async Task<ResultModel> GetUomUomForSelect(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productProduct = _dbContext.ProductProduct
                .Include(_ => _.ProductTemplate)
                .ThenInclude(_ => _.UomUom)
                .FirstOrDefault(_ => _.Id == id);
            if(productProduct == null)
            {
                throw new Exception("Product Variant not exists");

            }
            var uomUom = _dbContext.UomUom.Include(_ => _.Category).Where(_ => _.CategoryId == productProduct.ProductTemplate.UomUom.CategoryId).AsQueryable();
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<UomUomModel>(uomUom).ToList();
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

}
