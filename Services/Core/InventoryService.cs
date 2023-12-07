using AutoMapper;
using Data.DataAccess;
using Data.Entities;
using Data.Model;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Utils;

namespace Services.Core;

public interface IInventoryService
{
    Task<ResultModel> GetBarcode(Guid id);
    Task<ResultModel> GetDetail(Guid id);
    Task<ResultModel> UpdateLocation(UpdateLocationModel model);
}
public class InventoryService : IInventoryService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;

    public InventoryService(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ResultModel> GetBarcode(Guid id)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var data = _dbContext.Inventory.Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Inventory not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = MyFunction.GenerateBarcode(content: data.Id.ToString());
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
            var data = _dbContext.Inventory
                .Include(_ => _.Product)
                .Include(_ => _.InventoryLocations).ThenInclude(_ => _.Location).ThenInclude(_ => _.RackLevel).ThenInclude(_ => _.Rack)
                .Where(_ => _.Id == id && !_.IsDeleted).FirstOrDefault();
            if (data == null)
            {
                result.ErrorMessage = "Inventory not exists";
                result.Succeed = false;
                return result;
            }
            result.Succeed = true;
            result.Data = _mapper.Map<InventoryFIModel>(data);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }

    public async Task<ResultModel> UpdateLocation(UpdateLocationModel model)
    {
        var result = new ResultModel();
        result.Succeed = false;
        try
        {
            var inventory = _dbContext.Inventory.Include(_ => _.InventoryLocations).Where(_ => _.Id == model.Id && !_.IsDeleted).FirstOrDefault();
            if (inventory == null)
            {
                result.ErrorMessage = "Inventory not exists";
                result.Succeed = false;
                return result;
            }

            var location = _dbContext.Location.Where(_ => _.Id == model.LocationId && !_.IsDeleted).FirstOrDefault();
            if (location == null)
            {
                result.ErrorMessage = "Location not exists";
                result.Succeed = false;
                return result;
            }

            var inventoryLocationCheck = _dbContext.InventoryLocation.Where(_ => _.LocationId == location.Id && _.InventoryId == inventory.Id && !_.IsDeleted).FirstOrDefault();
            if (inventoryLocationCheck != null)
            {
                result.ErrorMessage = "Inventory already exists at this location";
                result.Succeed = false;
                return result;
            }

            _dbContext.InventoryLocation.Remove(inventoryLocationCheck!);
            var inventoryLocation = new InventoryLocation { InventoryId = inventory.Id, LocationId = location.Id };
            _dbContext.Add(inventoryLocation);
            _dbContext.SaveChanges();
            result.Succeed = true;
            result.Data = _mapper.Map<InventoryLocationFIModel>(inventoryLocation);
        }
        catch (Exception ex)
        {
            result.ErrorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
        }
        return result;
    }
}
