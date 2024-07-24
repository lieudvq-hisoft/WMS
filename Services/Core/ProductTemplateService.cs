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
    Task<ResultModel> SuggestProductVariants(Guid id);
    Task<ResultModel> CreateProductVariant(ProductVariantCreate model);
    Task<ResultModel> GetProductVariant(PagingParam<ProductVariantSortCriteria> paginationModel, Guid id);
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
                    Pvcs = _.ProductVariantCombinations.Select(pvc =>
                    new Pvc
                    {
                        Attribute = pvc.ProductTemplateAttributeValue.ProductAttributeValue.ProductAttribute.Name,
                        Value = pvc.ProductTemplateAttributeValue.ProductAttributeValue.Name
                    })
                    .ToList()
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
                .ThenInclude(_ => _.ProductVariantCombinations)
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
                    var currentPtavIds = productCurrent.ProductVariantCombinations.Select(_ => _.ProductTemplateAttributeValueId).ToList();
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
}
