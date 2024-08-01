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
public class StockPickingController : ControllerBase
{
    private readonly IStockPickingService _stockPickingService;
    public StockPickingController(IStockPickingService stockPickingService)
    {
        _stockPickingService = stockPickingService;
    }

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagingParam<SortStockPickingCriteria> paginationModel)
    {
        var result = await _stockPickingService.Get(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] StockPickingCreate model)
    {
        var result = await _stockPickingService.Create(model, Guid.Parse(User.GetId()));
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> Put([FromBody] StockPickingUpdate model)
    {
        var result = await _stockPickingService.Update(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _stockPickingService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
