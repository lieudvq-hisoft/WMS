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

public interface IProductCategoryService
{
    Task<ResultModel> Create(ProductCategoryCreate model);
    Task<ResultModel> Update(ProductCategoryUpdate model);
    Task<ResultModel> UpdateParent(ProductCategoryParentUpdate model);
    Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel);
    Task<ResultModel> Delete(Guid id);
}
public class ProductCategoryService : IProductCategoryService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public ProductCategoryService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(ProductCategoryCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {

            var productCategory = _mapper.Map<ProductCategoryCreate, ProductCategory>(model);
            if (productCategory.ParentId != null)
            {
                var parentCategory = _dbContext.ProductCategory.FirstOrDefault(_ => _.Id == productCategory.ParentId);
                if (parentCategory == null)
                {
                    throw new Exception("Parent Category not exists");
                }
                productCategory.CompleteName = $"{parentCategory.CompleteName} / {productCategory.Name}";
            }
            else
            {
                productCategory.CompleteName = productCategory.Name;
            }
            _dbContext.Add(productCategory);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<ProductCategory, ProductCategoryModel>(productCategory);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(ProductCategoryUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var productCategory = _dbContext.ProductCategory.Include(_ => _.ParentCategory).FirstOrDefault(_ => _.Id == model.Id);
                if (productCategory == null)
                {
                    throw new Exception("Product Category not exists");
                }
                if (model.Name != null)
                {
                    productCategory.Name = model.Name;
                    ComputeCompleteName(productCategory);
                    UpdateCompleteNamesRecursive(_dbContext, productCategory.Id);
                }
                productCategory.WriteDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                result.Succeed = true;
                result.Data = _mapper.Map<ProductCategory, ProductCategoryModel>(productCategory);
                    
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

    public async Task<ResultModel> UpdateParent(ProductCategoryParentUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var newParentCategory = _dbContext.ProductCategory.FirstOrDefault(_ => _.Id == model.ParentId);
                if (newParentCategory == null)
                {
                    throw new Exception("New parent Product Category not exists");
                }
                var productCategory = _dbContext.ProductCategory.FirstOrDefault(_ => _.Id == model.Id);
                if (productCategory == null)
                {
                    throw new Exception("Product Category not exists");
                }
                productCategory.ParentId = newParentCategory.Id;
                await _dbContext.SaveChangesAsync();

                ComputeCompleteName(productCategory);
                UpdateCompleteNamesRecursive(_dbContext, productCategory.Id);

                productCategory.WriteDate = DateTime.Now;

                await _dbContext.SaveChangesAsync();

                result.Succeed = true;
                result.Data = _mapper.Map<ProductCategory, ProductCategoryModel>(productCategory);

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

    public async Task<ResultModel> Get(PagingParam<SortCriteria> paginationModel)
    {
        var result = new ResultModel();
        try
        {
            var productCategories = _dbContext.ProductCategory.AsQueryable();
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, productCategories.Count());
            productCategories = productCategories.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            productCategories = productCategories.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ProductCategoryModel>(productCategories);
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
            var productCategory = _dbContext.ProductCategory.FirstOrDefault(_ => _.Id == id);
            if (productCategory == null)
            {
                throw new Exception("Product Category not exists");
            }
            _dbContext.Remove(productCategory);
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

    private async void ComputeCompleteName(ProductCategory category)
    {
        if (category.ParentCategory != null)
        {
            category.CompleteName = $"{category.ParentCategory.CompleteName} / {category.Name}";
        }
        else
        {
            category.CompleteName = category.Name;
        }
    }

    private async void UpdateCompleteNamesRecursive(AppDbContext dbContext, Guid Id)
    {
        foreach (var child in dbContext.ProductCategory.Include(_ => _.ParentCategory).Where(c => c.ParentId == Id).ToList())
        {
            if(child.ParentCategory != null)
            {
                child.CompleteName = $"{child.ParentCategory.CompleteName} / {child.Name}";
            }

            UpdateCompleteNamesRecursive(dbContext, child.Id);
        }
    }
}
