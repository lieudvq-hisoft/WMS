using AutoMapper;
using Data.Common.PaginationModel;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Data.Models;
using Data.Utils.Paging;
using Microsoft.EntityFrameworkCore;
using Services.Utils;

namespace Services.Core;

public interface IProductService
{
    Task<ResultModel> Create(ProductCreateModel model);
    Task<ResultModel> Update(ProductUpdateModel model);
    Task<ResultModel> Get(PagingParam<ProductSortCriteria> paginationModel, ProductSearchModel model);
    Task<ResultModel> Delete(Guid id);
}
public class ProductService : IProductService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public ProductService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> Create(ProductCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var supplier = _dbContext.Supplier.Where(_ => _.Id == model.SupplierId && !_.IsDeleted).FirstOrDefault();
            if (supplier == null)
            {
                result.ErrorMessage = "Supplier not exists";
                result.Succeed = false;
                return result;
            }
            var data = _mapper.Map<ProductCreateModel, Product>(model);
            _dbContext.Product.Add(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = data.Id;
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
            var data = _dbContext.Product.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.Product.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = data.Id;

        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Get(PagingParam<ProductSortCriteria> paginationModel, ProductSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Include(_ => _.Supplier).Where(delegate (Product p)
            {
                if (
                    (MyFunction.ConvertToUnSign(p.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    ||
                    (MyFunction.ConvertToUnSign(p.Supplier.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (p.Supplier.Phone.ToUpper().Contains(model.SearchValue ?? "".ToUpper())
                    || (p.Supplier.Email.ToUpper().Contains(Uri.UnescapeDataString(model.SearchValue ?? "").ToUpper())
                    )))
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var products = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            products = products.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<ProductModel>(products);
            paging.Data = viewModels;
            result.Data = paging;
            result.Succeed = true;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(ProductUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Product.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            if (model.Name != null)
            {
                data!.Name = model.Name;
            }

            if (model.Description != null)
            {
                data!.Description = model.Description;
            }

            if (model.UnitPrice != null)
            {
                data!.UnitPrice = model.UnitPrice;
            }

            if (model.CostPrice != null)
            {
                data!.CostPrice = model.CostPrice;
            }

            if (model.Status != null)
            {
                data!.Status = model.Status;
            }

            if (model.InventoryCount != null)
            {
                data!.InventoryCount = model.InventoryCount;
            }

            if (model.Status != null)
            {
                data!.Status = model.Status;
            }

            data!.DateUpdated = DateTime.Now;
            _dbContext.Product.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<Product, ProductModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
