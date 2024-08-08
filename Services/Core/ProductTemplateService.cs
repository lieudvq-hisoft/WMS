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
using Services.Utils;

namespace Services.Core;

public interface IProductTemplateService
{
    Task<ResultModel> Create(ProductTemplateCreate model);
    Task<ResultModel> Update(ProductTemplateUpdate model);
    Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetInfo(Guid id);
    Task<ResultModel> GetAttributeLine(PagingParam<ProductTemplateAttributeLineSortCriteria> paginationModel, Guid id);
    Task<ResultModel> SuggestProductVariants(Guid id);
    Task<ResultModel> CreateProductVariant(ProductVariantCreate model);
    Task<ResultModel> GetProductVariant(PagingParam<ProductVariantSortCriteria> paginationModel, Guid id);
    Task<ResultModel> GetStockQuant(PagingParam<StockQuantSortCriteria> paginationModel, Guid id);
    Task<ResultModel> GetProductVariantForSelect(Guid id);
    Task<ResultModel> UpdateImage(ProductTemplateImageUpdate model, Guid id);

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
            var productTemplate = _dbContext.ProductTemplate
                .Include(_ => _.ProductProducts)
                    .ThenInclude(_ => _.StockQuants)
                .FirstOrDefault(_ => _.Id == model.Id);
            if (productTemplate == null)
            {
                throw new Exception("Product Template not exists");
            }
            if (model.CategId != null)
            {
                productTemplate.CategId = (Guid)model.CategId;
            }
            if (model.UomId != null && model.UomId != productTemplate.UomId)
            {
                bool hasStockQuant = productTemplate.ProductProducts
                    .Any(product => product.StockQuants.Any(stockQuant => stockQuant.Quantity > 0));
                if (hasStockQuant)
                {
                    throw new Exception("You cannot change the unit of measure as there are already stock moves for this product. If you want to change the unit of measure, you should rather archive this product and create a new one.");
                }

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
            var productTemplates = _dbContext.ProductTemplate
                .Include(_ => _.ProductCategory)
                .Include(_ => _.UomUom)
                .Include(_ => _.ProductProducts)
                .ThenInclude(_ => _.StockQuants)
                .AsQueryable();
            if (!string.IsNullOrEmpty(paginationModel.SearchText))
            {
                productTemplates = productTemplates.Where(_ => _.Name.Contains(paginationModel.SearchText));
            }
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, productTemplates.Count());
            productTemplates = productTemplates.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            productTemplates = productTemplates.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            //var viewModels = _mapper.ProjectTo<ProductTemplateInfo>(productTemplates);
            var viewModels = productTemplates.Select(pt => new ProductTemplateInfo
            {
                Id = pt.Id,
                Name = pt.Name,
                ProductCategory = _mapper.Map<ProductCategoryModel>(pt.ProductCategory),
                UomUom = _mapper.Map<UomUomModel>(pt.UomUom),
                TotalVariant = pt.ProductProducts.Count(),
                QtyAvailable = pt.ProductProducts.SelectMany(pp => pp.StockQuants).Sum(sq => sq.Quantity),
                ImageUrl = pt.ImageUrl,
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

    public async Task<ResultModel> GetProductVariant(PagingParam<ProductVariantSortCriteria> paginationModel, Guid id)
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
                .Include(_ => _.StockQuants)
                .Where(_ => _.ProductTmplId == id).AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, productProducts.Count());
            productProducts = productProducts.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            productProducts = productProducts.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = productProducts
                .Select(_ =>
                new ProductProductModel
                {
                    Id = _.Id,
                    Name = _.ProductTemplate.Name,
                    Pvcs = _.ProductVariantCombinations.OrderBy(pvc => pvc.CreateDate).Select(pvc =>
                    new Pvc
                    {
                        Attribute = pvc.ProductTemplateAttributeValue.ProductAttributeValue.ProductAttribute.Name,
                        Value = pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name
                    })
                    .ToList(),
                    QtyAvailable = _.StockQuants.Sum(sq => sq.Quantity),
                    UomUom = _.ProductTemplate.UomUom.Name
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
                .Where(sq => sq.ProductProduct.ProductTemplate.Id == id && sq.StockLocation.Usage == LocationType.Internal)
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
                        Pvcs = _.ProductProduct.ProductVariantCombinations.OrderBy(pvc => pvc.CreateDate).Select(pvc =>
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
                    InventoryQuantity= _.InventoryQuantity,
                    InventoryDiffQuantity= _.InventoryDiffQuantity,
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
            var productTemplate = _dbContext.ProductTemplate
                .Include(_ => _.ProductCategory)
                .Include(_ => _.ProductProducts)
                .ThenInclude(_ => _.StockQuants)
                .Include(_ => _.UomUom).FirstOrDefault(_ => _.Id == id);
            if (productTemplate == null)
            {
                throw new Exception("Product Category not exists");
            }
            result.Succeed = true;
            var data = _mapper.Map<ProductTemplateInfo>(productTemplate);
            data.TotalVariant = productTemplate.ProductProducts.Count();
            result.Data = data;
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
                .Include(_ => _.ProductAttribute).ThenInclude(_ => _.ProductAttributeValues)
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

    public async Task<ResultModel> SuggestProductVariants(Guid id)
    {
        var result = new ResultModel();
        try
        {
            var productTemplate = _dbContext.ProductTemplate
                .Include(_ => _.ProductTemplateAttributeLines)
                .ThenInclude(_ => _.ProductTemplateAttributeValues)
                .ThenInclude(_ => _.ProductAttributeValue)
                .ThenInclude(_ => _.ProductAttribute)
                .Include(_ => _.ProductProducts)
                .ThenInclude(_ => _.ProductVariantCombinations.OrderBy(pvc => pvc.CreateDate))
                .Where(_ => _.Id == id).FirstOrDefault();
            if(productTemplate == null)
            {
                throw new Exception("Product Template not exists");
            }

            var suggestPvcs = productTemplate.ProductTemplateAttributeLines
                .Select(_ => _.ProductTemplateAttributeValues.Select(ptav =>
                new ProductVariantCombinationSuggest
                {
                    ProductTemplateAttributeValueId = ptav.Id,
                    AttributeName = ptav.ProductAttributeValue.ProductAttribute.Name,
                    AttributeValue = ptav.ProductAttributeValue.Name,
                }).ToList()).ToList();

            var combinationSuggests = CartesianProduct(suggestPvcs);

            var combinationSuggestResults = new List<List<ProductVariantCombinationSuggest>>();

            foreach (var combinationSuggest in combinationSuggests)
            {
                combinationSuggestResults.Add(combinationSuggest);
                var suggestPvcSet = new HashSet<Guid>(combinationSuggest.Select(_ => _.ProductTemplateAttributeValueId));
                foreach (var productCurrent in productTemplate.ProductProducts)
                {
                    var currentPtavIds = productCurrent.ProductVariantCombinations.OrderBy(pvc => pvc.CreateDate).Select(_ => _.ProductTemplateAttributeValueId).ToList();
                    var currentPtavIdSet = new HashSet<Guid>(currentPtavIds);

                    if (suggestPvcSet.SetEquals(currentPtavIdSet))
                    {
                        combinationSuggestResults.Remove(combinationSuggest);

                    }
                }


            }
            result.Succeed = true;
            result.Data = combinationSuggestResults;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> CreateProductVariant(ProductVariantCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var productTemplate = _dbContext.ProductTemplate
                    .Include(_ => _.ProductProducts).ThenInclude(_ => _.ProductVariantCombinations)
                    .FirstOrDefault(_ => _.Id == model.ProductTemplateId);
                if (productTemplate == null)
                {
                    throw new Exception("Product Template not exists");
                }

                var requestPtavIdSet = new HashSet<Guid>(model.PtavIds);

                foreach (var productCurrent in productTemplate.ProductProducts)
                {
                    var currentPtavIds = productCurrent.ProductVariantCombinations.Select(_ => _.ProductTemplateAttributeValueId).ToList();
                    var currentPtavIdSet = new HashSet<Guid>(currentPtavIds);
                    if (requestPtavIdSet.SetEquals(currentPtavIdSet))
                    {
                        throw new Exception("Product Variations already exist");
                    }
                }

                var productProductCreate = new ProductProductCreate { ProductTmplId = productTemplate.Id };
                var productProduct = _mapper.Map<ProductProductCreate, ProductProduct>(productProductCreate);
                _dbContext.ProductProduct.Add(productProduct);

                foreach (var productTemplateAttributeValueId in model.PtavIds)
                {
                    var productTemplateAttributeValue = _dbContext.ProductTemplateAttributeValue.Include(_ => _.ProductTemplateAttributeLine)
                                .FirstOrDefault(_ => _.ProductTemplateAttributeLine.ProductTmplId == productTemplate.Id);
                    if (productTemplateAttributeValue == null)
                    {
                        throw new Exception("Product Template Attribute Value not exists");
                    }

                    var productVariantCombinationCreate = new ProductVariantCombinationCreate
                    {
                        ProductProductId = productProduct.Id,
                        ProductTemplateAttributeValueId = productTemplateAttributeValueId
                    };
                    var productVariantCombination = _mapper.Map<ProductVariantCombinationCreate, ProductVariantCombination>(productVariantCombinationCreate);
                    _dbContext.ProductVariantCombination.Add(productVariantCombination);
                }

                await _dbContext.SaveChangesAsync();

                result.Succeed = true;
                result.Data = productProduct.Id;

                await transaction.CommitAsync();

            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                await transaction.RollbackAsync();
            }
        }
        return result;
    }

    public async Task<ResultModel> GetProductVariantForSelect(Guid id)
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
                .Include(_ => _.StockQuants)
                .Where(_ => _.ProductTmplId == id).AsQueryable();
            var viewModels = productProducts
                .Select(_ =>
                new ProductProductModel
                {
                    Id = _.Id,
                    Name = _.ProductTemplate.Name,
                    Pvcs = _.ProductVariantCombinations.OrderBy(pvc => pvc.CreateDate).Select(pvc =>
                    new Pvc
                    {
                        Attribute = pvc.ProductTemplateAttributeValue.ProductAttributeValue.ProductAttribute.Name,
                        Value = pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name
                    })
                    .ToList(),
                    QtyAvailable = _.StockQuants.Sum(sq => sq.Quantity),
                    UomUom = _.ProductTemplate.UomUom.Name
                });
            result.Succeed = true;
            result.Data = viewModels;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public List<List<T>> CartesianProduct<T>(List<List<T>> sequences)
    {
        List<List<T>> result = new List<List<T>>();
        var count = sequences.Count;
        if (count == 0)
        {
            return result;
        }
        var indexes = new int[count];
        while (true)
        {
            List<T> current = new List<T>();
            for (int i = 0; i < count; i++)
            {
                current.Add(sequences[i][indexes[i]]);
            }
            result.Add(current);

            int incrementIndex = count - 1;
            while (incrementIndex >= 0 && ++indexes[incrementIndex] == sequences[incrementIndex].Count)
            {
                indexes[incrementIndex] = 0;
                incrementIndex--;
            }
            if (incrementIndex < 0)
            {
                break;
            }
        }
        return result;
    }

    public async Task<ResultModel> UpdateImage(ProductTemplateImageUpdate model, Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var productTemplate = _dbContext.ProductTemplate.FirstOrDefault(_ => _.Id == id);
            if (productTemplate == null)
            {
                result.ErrorMessage = "Product Template not exist!";
                return result;
            }
            if (productTemplate.ImageUrl != null)
            {
                string dirPathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                MyFunction.DeleteFile(dirPathDelete + productTemplate.ImageUrl);
            }
            string dirPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Product", productTemplate.Id.ToString());
            productTemplate.ImageUrl = await MyFunction.UploadImageAsync(model.File, dirPath);
            productTemplate.WriteDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = productTemplate.ImageUrl;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
