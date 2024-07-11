﻿using AutoMapper;
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
        try
        {
            var uomUom = _dbContext.UomUom.Include(_ => _.Category).ThenInclude(_ => _.UomUoms).FirstOrDefault(_ => _.Id == model.Id);
            if (uomUom == null)
            {
                throw new Exception("UomUom not exists");
            }
            if (model.UomType.ToString() == "Reference")
            {
                uomUom.Category.UpdateReferenceUom(uomUom.Id);
                uomUom.UomType = model.UomType.ToString();
            }else
            {
                uomUom.UomType = model.UomType.ToString();
                uomUom.Category.UpdateReferenceUom(uomUom.Id);
            }
            uomUom.WriteDate = DateTime.Now;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<UomUom, UomUomModel>(uomUom);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateFactor(UomUomUpdateFactor model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var uomUom = _dbContext.UomUom.Include(_ => _.Category).ThenInclude(_ => _.UomUoms).FirstOrDefault(_ => _.Id == model.Id);
            if (uomUom == null)
            {
                throw new Exception("UomUom not exists");
            }
            uomUom.Ratio = (decimal)model.Factor;
            uomUom.WriteDate = DateTime.Now;
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<UomUom, UomUomModel>(uomUom);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

}
