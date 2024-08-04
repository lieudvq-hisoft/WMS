using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Services.Core;

public interface IUomUomService
{
    Task<ResultModel> Create(UomUomCreate model);
    Task<ResultModel> UpdateType(UomUomUpdateType model);
    Task<ResultModel> UpdateFactor(UomUomUpdateFactor model);
    Task<ResultModel> UpdateInfo(UomUomUpdate model);
    Task<ResultModel> Delete(Guid id);
    Task<ResultModel> GetForSelect();
}
public class UomUomService : IUomUomService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;

    public UomUomService(AppDbContext dbContext, IMapper mapper, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ResultModel> Create(UomUomCreate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var uomCate = _dbContext.UomCategory.Include(_ => _.UomUoms).FirstOrDefault(_ => _.Id == model.CategoryId);
            if (uomCate == null)
            {
                throw new Exception("Uom Category not exists");
            }
            var uomUom = _mapper.Map<UomUomCreate, UomUom>(model);
            if(uomUom.UomType == "Reference")
            {
                uomUom.Factor = 1;
            }
            uomCate.UomUoms.Add(uomUom);
            uomCate.UpdateReferenceUom(uomUom.Id);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = uomUom.Id;
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateType(UomUomUpdateType model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var uomUom = _dbContext.UomUom
                    .Include(_ => _.StockMoves)
                    .Include(_ => _.Category).ThenInclude(_ => _.UomUoms).ThenInclude(_ => _.StockMoves)
                    .FirstOrDefault(_ => _.Id == model.Id);
                if (uomUom == null)
                {
                    throw new Exception("UomUom not exists");
                }
                bool hasStockMove = uomUom.StockMoves.Any();
                if (hasStockMove)
                {
                    throw new Exception("You cannot change the ratio of this unit of measure as some products with this UoM have already been moved or are currently reserved.");
                }
                uomUom.UomType = model.UomType.ToString();
                uomUom.Category.UpdateReferenceUom(uomUom.Id);
                uomUom.WriteDate = DateTime.Now;
                _dbContext.SaveChanges();
                result.Succeed = true;
                result.Data = _mapper.Map<UomUom, UomUomModel>(uomUom);
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

    public async Task<ResultModel> UpdateFactor(UomUomUpdateFactor model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var uomUom = _dbContext.UomUom.Include(_ => _.StockMoves).FirstOrDefault(_ => _.Id == model.Id);
            if (uomUom == null)
            {
                throw new Exception("UomUom not exists");
            }
            bool hasStockMove = uomUom.StockMoves.Any();
            if (hasStockMove)
            {
                throw new Exception("You cannot change the ratio of this unit of measure as some products with this UoM have already been moved or are currently reserved.");
            }
            if (uomUom.UomType != "Reference")
            {
                uomUom.Ratio = (decimal)model.Factor;
                uomUom.WriteDate = DateTime.Now;
                _dbContext.SaveChanges();
            }
            result.Succeed = true;
            result.Data = _mapper.Map<UomUom, UomUomModel>(uomUom);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateInfo(UomUomUpdate model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var uomUom = _dbContext.UomUom.FirstOrDefault(_ => _.Id == model.Id);
            if (uomUom == null)
            {
                throw new Exception("UomUom not exists");
            }
            if(model.Name != null)
            {
                uomUom.Name = model.Name;
            }
            if (model.Rounding != null)
            {
                uomUom.Rounding = (decimal)model.Rounding;
            }
            if (model.Active != null)
            {
                uomUom.Active = model.Active;
            }
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<UomUomModel>(uomUom);
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
            var uomUom = _dbContext.UomUom.FirstOrDefault(_ => _.Id == id);
            if (uomUom == null)
            {
                throw new Exception("UomUom not exists");
            }
            if(uomUom.UomType == "Reference")
            {
                var totalUomUom = _dbContext.UomUom.Count(_ => _.CategoryId == uomUom.CategoryId);
                if(totalUomUom > 1)
                {
                    throw new Exception("UomUom has type Reference, cannot be deleted!");
                }
            }
            _dbContext.Remove(uomUom);
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

    public async Task<ResultModel> GetForSelect()
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var uomUom = _dbContext.UomUom.Include(_ => _.Category).AsQueryable();
            result.Succeed = true;
            result.Data = _mapper.ProjectTo<UomUomModel>(uomUom).ToList();
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
