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

    [HttpGet("Incoming/{warehouseId}")]
    public async Task<ActionResult> GetStockPickingIncoming([FromQuery] PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId)
    {
        var result = await _stockPickingService.GetStockPickingIncoming(paginationModel, warehouseId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Internal/{warehouseId}")]
    public async Task<ActionResult> GetStockPickingInternal([FromQuery] PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId)
    {
        var result = await _stockPickingService.GetStockPickingInternal(paginationModel, warehouseId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Outgoing/{warehouseId}")]
    public async Task<ActionResult> GetStockPickingOutgoing([FromQuery] PagingParam<SortStockPickingCriteria> paginationModel, Guid warehouseId)
    {
        var result = await _stockPickingService.GetStockPickingOutgoing(paginationModel, warehouseId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("Receipt")]
    public async Task<ActionResult> Create([FromBody] StockPickingReceipt model)
    {
        var result = await _stockPickingService.CreateReceipt(model, Guid.Parse(User.GetId()));
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Info")]
    public async Task<ActionResult> GetInfo(Guid id)
    {
        var result = await _stockPickingService.GetInfo(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
