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

public interface IRackLevelService
{
    Task<ResultModel> Create(RackLevelCreateModel model);
    Task<ResultModel> Update(RackLevelUpdateModel model);
    Task<ResultModel> Get(PagingParam<RackLevelSortCriteria> paginationModel, RackLevelSearchModel model);
    Task<ResultModel> Delete(Guid id);
}
public class RackLevelService : IRackLevelService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public RackLevelService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> Create(RackLevelCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var rack = _dbContext.Rack.Where(_ => _.Id == model.RackId && !_.IsDeleted).FirstOrDefault();
            if (rack == null)
            {
                result.ErrorMessage = "Rack not exists";
                result.Succeed = false;
                return result;
            }
            var data = _mapper.Map<RackLevelCreateModel, RackLevel>(model);
            _dbContext.RackLevel.Add(data);
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
            var data = _dbContext.RackLevel.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Rack level not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.RackLevel.Update(data);
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

    public async Task<ResultModel> Get(PagingParam<RackLevelSortCriteria> paginationModel, RackLevelSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.RackLevel.Include(_ => _.Rack).Include(_ => _.Locations).Where(delegate (RackLevel r)
            {
                if (
                    (MyFunction.ConvertToUnSign(r.Description ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    )
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var rackLevels = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            rackLevels = rackLevels.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<RackLevelModel>(rackLevels);
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

    public async Task<ResultModel> Update(RackLevelUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.RackLevel.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Rack level not exists";
                result.Succeed = false;
                return result;
            }
            if (model.LevelNumber != null)
            {
                data!.LevelNumber = (int)model.LevelNumber;
            }

            if (model.Description != null)
            {
                data!.Description = model.Description;
            }

            data!.DateUpdated = DateTime.Now;
            _dbContext.RackLevel.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<RackLevel, RackLevelModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
