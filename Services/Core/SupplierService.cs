using AutoMapper;
using Data.Common.PaginationModel;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Data.Models;
using Data.Utils.Paging;
using Services.Utils;

namespace Services.Core;

public interface ISupplierService
{
    Task<ResultModel> Create(SupplierCreateModel model);
    Task<ResultModel> Update(SupplierUpdateModel model);
    Task<ResultModel> Get(PagingParam<SupplierSortCriteria> paginationModel, SupplierSearchModel model);
    Task<ResultModel> Delete(Guid id);
}
public class SupplierService : ISupplierService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public SupplierService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> Create(SupplierCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _mapper.Map<SupplierCreateModel, Supplier>(model);
            _dbContext.Supplier.Add(data);
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
            var data = _dbContext.Supplier.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Supplier not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.Supplier.Update(data);
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

    public async Task<ResultModel> Get(PagingParam<SupplierSortCriteria> paginationModel, SupplierSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Supplier.Where(delegate (Supplier s)
            {
                if (
                    (MyFunction.ConvertToUnSign(s.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0) ||
                    (MyFunction.ConvertToUnSign(s.Address ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    || (s.Phone.ToUpper().Contains(model.SearchValue ?? "".ToUpper())
                    || (s.Email.ToUpper().Contains(Uri.UnescapeDataString(model.SearchValue ?? "").ToUpper())
                    )))
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var suppliers = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            suppliers = suppliers.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<SupplierModel>(suppliers);
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

    public async Task<ResultModel> Update(SupplierUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Supplier.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if(data == null)
            {
                result.ErrorMessage = "Supplier not exists";
                result.Succeed = false;
                return result;
            }
            if (model.Address != null)
            {
                data!.Address = model.Address;
            }

            if (model.Name != null)
            {
                data!.Name = model.Name;
            }
            if (model.StaffName != null)
            {
                data!.StaffName = model.StaffName;
            }
            if (model.Position != null)
            {
                data!.Position = model.Position;
            }
            if (model.Phone != null)
            {
                data!.Phone = model.Phone;
            }

            if (model.Email != null)
            {
                data!.Email = model.Email;
            }

            if (model.Status != null)
            {
                data!.Status = model.Status;
            }

            data!.DateUpdated = DateTime.Now;
            _dbContext.Supplier.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<Supplier, SupplierModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
