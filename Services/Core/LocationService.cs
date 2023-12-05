using AutoMapper;
using Data.Common.PaginationModel;
using Data.DataAccess;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Data.Models;
using Data.Utils.Paging;
using Microsoft.EntityFrameworkCore;
using QRCoder;
using Services.Utils;

namespace Services.Core;

public interface ILocationService
{
    Task<ResultModel> Create(LocationCreateModel model);
    Task<ResultModel> Update(LocationUpdateModel model);
    Task<ResultModel> Get(PagingParam<LocationSortCriteria> paginationModel, LocationSearchModel model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetBarcode(Guid id);
    Task<ResultModel> GetQrcode(Guid id);
    Task<ResultModel> GetDetail(Guid id);
}
public class LocationService : ILocationService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public LocationService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> Create(LocationCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var rackLevel = _dbContext.RackLevel.Where(_ => _.Id == model.RackLevelId && !_.IsDeleted).FirstOrDefault();
            if (rackLevel == null)
            {
                result.ErrorMessage = "Rack level not exists";
                result.Succeed = false;
                return result;
            }
            var data = _mapper.Map<LocationCreateModel, Location>(model);
            _dbContext.Location.Add(data);
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
            var data = _dbContext.Location.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Location not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.Location.Update(data);
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

    public async Task<ResultModel> Get(PagingParam<LocationSortCriteria> paginationModel, LocationSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Location
                .Include(_ => _.InventoryLocations).ThenInclude(_ => _.Inventory).ThenInclude(_ => _.Product)
                .Include(_ => _.RackLevel).Where(delegate (Location l)
            {
                if (
                    (MyFunction.ConvertToUnSign(l.Name ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    ||
                    (MyFunction.ConvertToUnSign(l.Description ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    )
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var locations = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            locations = locations.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<LocationModel>(locations);
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

    public async Task<ResultModel> GetBarcode(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Location.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Location not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = MyFunction.GenerateBarcode(data.Id.ToString(), width: 700);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetQrcode(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Location.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Location not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = MyFunction.GenerateQrcode(data.Id.ToString());
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> GetDetail(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Location.Include(_ => _.InventoryLocations).ThenInclude(_ => _.Inventory).ThenInclude(_ => _.Product)
                .Include(_ => _.RackLevel).Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Location not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = _mapper.Map<LocationModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> Update(LocationUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Location.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Location not exists";
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

            if (model.SectionNumber != null)
            {
                data!.SectionNumber = (int)model.SectionNumber;
            }

            data!.DateUpdated = DateTime.Now;
            _dbContext.Location.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<Location, LocationModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
