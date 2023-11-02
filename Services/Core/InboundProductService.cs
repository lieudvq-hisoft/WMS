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

public interface IInboundProductService
{
    Task<ResultModel> Create(InboundProductCreateModel model);
    Task<ResultModel> Update(InboundProductUpdateModel model);
    Task<ResultModel> Get(PagingParam<InboundProductSortCriteria> paginationModel, InboundProductSearchModel model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> Completed(InboundProductCompletedModel model);
}
public class InboundProductService : IInboundProductService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public InboundProductService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> Completed(InboundProductCompletedModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var data = _dbContext.InboundProduct.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Inbound Product not exists";
                result.Succeed = false;
                return result;
            }
            if(data.Status == InboundProductStatus.Completed)
            {
                result.ErrorMessage = "Inbound Product is completed";
                result.Succeed = false;
                return result;
            }
            if (data.Quantity <= 0)
            {
                result.ErrorMessage = "Inbound product cannot be completed with a quantity of 0 or less than 0";
                result.Succeed = false;
                return result;
            }
            var location = _dbContext.Location.Where(_ => _.Id == model.LocationId && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Location not exists";
                result.Succeed = false;
                return result;
            }

            data.Status = InboundProductStatus.Completed;
            data.DateUpdated = DateTime.Now;
            data.CompletedDate = DateTime.Now;
            _dbContext.InboundProduct.Update(data);

            var inventory = new Inventory
            {
                ProductId = data.ProductId,
                QuantityOnHand = data.Quantity,
                ReceivedDate = data.CompletedDate,
                Type = InventoryType.In,
                LocationId = location!.Id
            };
            _dbContext.Inventory.Add(inventory);

            _dbContext.SaveChanges();
            await transaction.CommitAsync();
            result.Succeed = true;
            result.Data = data.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            await transaction.RollbackAsync();
        }
        return result;
    }

    public async Task<ResultModel> Create(InboundProductCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var product = _dbContext.Product.Where(_ => _.Id == model.ProductId && !_.IsDeleted).FirstOrDefault();
            if (product == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }

            var receipt = _dbContext.Receipt.Where(_ => _.Id == model.ReceiptId && !_.IsDeleted).FirstOrDefault();
            if (receipt == null)
            {
                result.ErrorMessage = "Receipt not exists";
                result.Succeed = false;
                return result;
            }
            var data = _mapper.Map<InboundProductCreateModel, InboundProduct>(model);
            _dbContext.InboundProduct.Add(data);
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
            var data = _dbContext.InboundProduct.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "InboundProduct not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.InboundProduct.Update(data);
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

    public async Task<ResultModel> Get(PagingParam<InboundProductSortCriteria> paginationModel, InboundProductSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.InboundProduct.Include(_ => _.Product).Include(_ => _.Receipt).Where(delegate (InboundProduct i)
            {
                if (
                    (MyFunction.ConvertToUnSign(i.Product.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    )
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var inboundProducts = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            inboundProducts = inboundProducts.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<InboundProductModel>(inboundProducts);
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

    public async Task<ResultModel> Update(InboundProductUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.InboundProduct.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "InboundProduct not exists";
                result.Succeed = false;
                return result;
            }
            if (model.Quantity != null)
            {
                data!.Quantity = (int)model.Quantity;
            }

            if (model.PurchaseUnitPrice != null)
            {
                data!.PurchaseUnitPrice = model.PurchaseUnitPrice;
            }

            if (model.TotalCost != null)
            {
                data!.TotalCost = model.TotalCost;
            }

            if (model.Note != null)
            {
                data!.Note = model.Note;
            }

            if (model.Status != null)
            {
                data!.Status = (InboundProductStatus)model.Status;
            }

            if (model.ManufacturedDate != null)
            {
                data!.ManufacturedDate = model.ManufacturedDate;
            }

            if (model.ExpiredDate != null)
            {
                data!.ExpiredDate = model.ExpiredDate;
            }

            if (model.BatchNumber != null)
            {
                data!.BatchNumber = model.BatchNumber;
            }

            data!.DateUpdated = DateTime.Now;
            _dbContext.InboundProduct.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<InboundProduct, InboundProductModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
