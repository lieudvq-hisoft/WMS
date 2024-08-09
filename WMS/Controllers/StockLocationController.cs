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

    [HttpGet("Internal")]
    public async Task<ActionResult> GetInternalLocation()
    {
        var result = await _stockLocationService.GetInternalLocation();
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
}
