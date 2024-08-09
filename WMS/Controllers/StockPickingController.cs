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

    [HttpPut("Receipt")]
    public async Task<ActionResult> UpdateReceipt([FromBody] StockPickingUpdateReceipt model)
    {
        var result = await _stockPickingService.UpdateReceipt(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("DeliveryOrder")]
    public async Task<ActionResult> CreateDeliveryOrder([FromBody] StockPickingDeliveryOrder model)
    {
        var result = await _stockPickingService.CreateDeliveryOrder(model, Guid.Parse(User.GetId()));
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("DeliveryOrder")]
    public async Task<ActionResult> UpdateDeliveryOrder([FromBody] StockPickingUpdateDeliveryOrder model)
    {
        var result = await _stockPickingService.UpdateDeliveryOrder(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("InternalTransfer")]
    public async Task<ActionResult> CreateInternalTransfer([FromBody] StockPickingInternalTransfer model)
    {
        var result = await _stockPickingService.CreateInternalTransfer(model, Guid.Parse(User.GetId()));
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("InternalTransfer")]
    public async Task<ActionResult> UpdateInternalTransfer([FromBody] StockPickingUpdateInternalTransfer model)
    {
        var result = await _stockPickingService.UpdateInternalTransfer(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Info/{id}")]
    public async Task<ActionResult> GetInfo(Guid id)
    {
        var result = await _stockPickingService.GetInfo(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("StockMove/{id}")]
    public async Task<ActionResult> GetInfo([FromQuery] PagingParam<StockMoveSortCriteria> paginationModel, Guid id)
    {
        var result = await _stockPickingService.GetStockMoves(paginationModel, id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("MakeAsTodo/{id}")]
    public async Task<ActionResult> MakeAsTodo(Guid id)
    {
        var result = await _stockPickingService.MakeAsTodo(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Cancel/{id}")]
    public async Task<ActionResult> Cancel(Guid id)
    {
        var result = await _stockPickingService.Cancel(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Validate/{id}")]
    public async Task<ActionResult> Validate(Guid id)
    {
        var result = await _stockPickingService.Validate(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Validate/DeliveryOrder/{id}")]
    public async Task<ActionResult> ValidateDeliveryOrder(Guid id)
    {
        var result = await _stockPickingService.ValidateDeliveryOrder(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Validate/InternalTransfer/{id}")]
    public async Task<ActionResult> ValidateInternalTransfer(Guid id)
    {
        var result = await _stockPickingService.ValidateInternalTransfer(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
