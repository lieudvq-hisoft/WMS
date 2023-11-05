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

public interface IRackService
{
    Task<ResultModel> Create(RackCreateModel model);
    Task<ResultModel> Update(RackUpdateModel model);
    Task<ResultModel> Get(PagingParam<RackSortCriteria> paginationModel, RackSearchModel model);
    Task<ResultModel> Delete(Guid id);
}
public class RackService : IRackService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public RackService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> Create(RackCreateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _mapper.Map<RackCreateModel, Rack>(model);
            _dbContext.Rack.Add(data);
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
            var data = _dbContext.Rack.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Rack not exists";
                result.Succeed = false;
                return result;
            }
            data.IsDeleted = true;
            data.DateUpdated = DateTime.Now;
            _dbContext.Rack.Update(data);
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

    public async Task<ResultModel> Get(PagingParam<RackSortCriteria> paginationModel, RackSearchModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Rack.Include(_ => _.RackLevels).Where(delegate (Rack c)
            {
                if (
                    (MyFunction.ConvertToUnSign(c.Description ?? "").IndexOf(MyFunction.ConvertToUnSign(model.SearchValue ?? ""), StringComparison.CurrentCultureIgnoreCase) >= 0)
                    )
                    return true;
                else
                    return false;
            }).AsQueryable();
            data = data.Where(_ => !_.IsDeleted);
            var paging = new PagingModel(paginationModel.PageIndex, paginationModel.PageSize, data.Count());
            var racks = data.GetWithSorting(paginationModel.SortKey.ToString(), paginationModel.SortOrder);
            racks = racks.GetWithPaging(paginationModel.PageIndex, paginationModel.PageSize);
            var viewModels = _mapper.ProjectTo<RackModel>(racks);
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

    public async Task<ResultModel> Update(RackUpdateModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Rack.Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Rack not exists";
                result.Succeed = false;
                return result;
            }
            if (model.RackNumber != null)
            {
                data!.RackNumber = (int)model.RackNumber;
            }

            if (model.Description != null)
            {
                data!.Description = model.Description;
            }

            if (model.TotalLevel != null)
            {
                data!.TotalLevel = (int)model.TotalLevel;
            }

            data!.DateUpdated = DateTime.Now;
            _dbContext.Rack.Update(data);
            await _dbContext.SaveChangesAsync();
            result.Succeed = true;
            result.Data = _mapper.Map<Rack, RackModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
