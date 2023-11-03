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

public interface IPickingRequestService
{
    Task<ResultModel> Create(PickingRequestCreateModel model);
    Task<ResultModel> Update(PickingRequestUpdateModel model);
    Task<ResultModel> Get(PagingParam<PickingRequestSortCriteria> paginationModel, PickingRequestSearchModel model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> Complete(PickingRequestCompleteModel model);
}
public class PickingRequestService : IPickingRequestService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public PickingRequestService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> Complete(PickingRequestCompleteModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        var transaction = _dbContext.Database.BeginTransaction();
        try
        {
            var pickingRequest = _dbContext.PickingRequest.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (pickingRequest == null)
            {
                result.ErrorMessage = "Picking request not exists";
                result.Succeed = false;
                return result;
            }
            if (pickingRequest.Status == PickingRequestStatus.Completed)
            {
                result.ErrorMessage = "Picking request is completed";
                result.Succeed = false;
                return result;
            }
            if (model.pickingRequestInventories.Any())
            {
                foreach (var pickingRequestInventory in model.pickingRequestInventories)
                {
                    var inventoryCheck = _dbContext.Inventory.Where(_ => _.Id == pickingRequestInventory.InventoryId && !_.IsDeleted).FirstOrDefault();
                    if(inventoryCheck == null)
                    {
                        result.ErrorMessage = "Inventory not exists with Id: " + pickingRequestInventory.InventoryId;
                        result.Succeed = false;
                        await transaction.RollbackAsync();
                        return result;
                    }
                    if(inventoryCheck.QuantityOnHand < pickingRequestInventory.Quantity)
                    {
                        result.ErrorMessage = "Out of stock inventory with Id: " + pickingRequestInventory.InventoryId;
                        result.Succeed = false;
                        await transaction.RollbackAsync();
                        return result;
                    }
                    inventoryCheck.QuantityOnHand -= pickingRequestInventory.Quantity;
                    inventoryCheck.DateUpdated = DateTime.Now;
                    _dbContext.Inventory.Update(inventoryCheck);
                    var pickingRequestInventoryAdd = new Data.Entities.PickingRequestInventory
                    {
                        InventoryId = inventoryCheck.Id,
                        PickingRequestId = pickingRequest.Id,
                        Quantity = pickingRequestInventory.Quantity,
                    };
                    _dbContext.PickingRequestInventory.Add(pickingRequestInventoryAdd);
                }
            }
            var pickingRequestAfterCheck = _dbContext.PickingRequest.Include(_ => _.PickingRequestInventories).Where(_ => _.Id == pickingRequest.Id && !_.IsDeleted).FirstOrDefault();
            var quantityAfterCheck = 0;
            foreach (var item in pickingRequestAfterCheck!.PickingRequestInventories)
            {
                quantityAfterCheck += item.Quantity;
            }
            if(quantityAfterCheck != pickingRequest.Quantity)
            {
                result.ErrorMessage = "Error";
                result.Succeed = false;
                await transaction.RollbackAsync();
                return result;
            }
            pickingRequest.Status = PickingRequestStatus.Completed;
            pickingRequest.DateUpdated = DateTime.Now;
            _dbContext.PickingRequest.Update(pickingRequest);
            await _dbContext.SaveChangesAsync();
            await transaction.CommitAsync();
            result.Succeed = true;
            result.Data = pickingRequest.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            await transaction.RollbackAsync();
        }
        return result;
    }

    public async Task<ResultModel> Create(PickingRequestCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var sentBy = _dbContext.User.Where(_ => _.Id == model.SentBy && !_.IsDeleted).FirstOrDefault();
            if (sentBy == null)
            {
                result.ErrorMessage = "User not exists";
                result.Succeed = false;
                return result;
            }
            var product = _dbContext.Product.Include(_ => _.Inventories).Where(_ => _.Id == model.ProductId && !_.IsDeleted).FirstOrDefault();
            if(product == null)
            {
                result.ErrorMessage = "Product not exists";
                result.Succeed = false;
                return result;
            }
            //check inventory
            var inventories = product.Inventories.Where(_ => _.QuantityOnHand > 0 && !_.IsDeleted);
            var totalInventory = 0;
            if (inventories.Any())
            {
                foreach (var item in inventories)
                {
                    totalInventory += item.QuantityOnHand;
                }
            }
            if(totalInventory < model.Quantity)
            {
                result.ErrorMessage = "Out of stock";
                result.Succeed = false;
                return result;
            }
            //end check inventory

            var data = _mapper.Map<PickingRequestCreateModel, PickingRequest>(model);
            _dbContext.PickingRequest.Add(data);
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
            var data = _dbContext.PickingRequest.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Picking request not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.PickingRequest.Update(data);
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

    public async Task<ResultModel> Get(PagingParam<PickingRequestSortCriteria> paginationModel, PickingRequestSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.PickingRequest.Include(_ => _.SentByUser).Where(delegate (PickingRequest p)
            {
                if (
                    (MyFunction.ConvertToUnSign(p.Note ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (p.SentByUser.UserName.ToUpper().Contains(model.SearchValue ?? "".ToUpper())
                    || (p.SentByUser.Email.ToUpper().Contains(Uri.UnescapeDataString(model.SearchValue ?? "").ToUpper())
                    )))
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var pickingRequests = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            pickingRequests = pickingRequests.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<PickingRequestModel>(pickingRequests);
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

    public async Task<ResultModel> Update(PickingRequestUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.PickingRequest.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Picking request not exists";
                result.Succeed = false;
                return result;
            }
            if (data.Status == PickingRequestStatus.Completed)
            {
                result.ErrorMessage = "Receipt is complete, cannot be updated";
                result.Succeed = false;
                return result;
            }
            if (model.Note != null)
            {
                data!.Note = model.Note;
            }
            if (model.Quantity != null)
            {
                data!.Quantity = (int)model.Quantity;
            }
            data!.DateUpdated = DateTime.Now;
            _dbContext.PickingRequest.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<PickingRequest, PickingRequestModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
