using Confluent.Kafka;
using Data.Common.PaginationModel;
using Data.Enums;
using Data.Model;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;
using Services.Utils;

namespace UomCategory.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class StockLocationController : ControllerBase
{
    private readonly IStockLocationService _stockLocationService;
    public StockLocationController(IStockLocationService stockLocationService)
    {
        _stockLocationService = stockLocationService;
    }

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagingParam<StockLocationSortCriteria> paginationModel)
    {
        var result = await _stockLocationService.Get(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Info/{id}")]
    public async Task<ActionResult> GetInfo(Guid id)
    {
        var result = await _stockLocationService.GetInfo(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("SelectParent/{id}")]
    public async Task<ActionResult> GetForSelectParent(Guid id)
    {
        var result = await _stockLocationService.GetForSelectParent(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("SelectParent")]
    public async Task<ActionResult> GetForSelectParent()
    {
        var result = await _stockLocationService.GetForSelectParent();
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Select")]
    public async Task<ActionResult> GetLocation()
    {
        var result = await _stockLocationService.GetLocation();
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _stockLocationService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("StockQuant/{id}")]
    public async Task<ActionResult> GetStockQuant([FromQuery] PagingParam<StockQuantSortCriteria> paginationModel, Guid id)
    {
        var result = await _stockLocationService.GetStockQuant(paginationModel, id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] StockLocationCreate model)
    {
        var result = await _stockLocationService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Parent")]
    public async Task<ActionResult> UpdateParent([FromBody] StockLocationParentUpdate model)
    {
        var result = await _stockLocationService.UpdateParent(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] StockLocationUpdate model)
    {
        var result = await _stockLocationService.Update(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Warehouse/{id}")]
    public async Task<ActionResult> GetLocationWarehouse(Guid id)
    {
        var result = await _stockLocationService.GetLocationWarehouse(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
