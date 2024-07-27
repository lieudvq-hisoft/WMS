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
public class StockPickingTypeController : ControllerBase
{
    private readonly IStockPickingTypeService _stockPickingTypeService;
    public StockPickingTypeController(IStockPickingTypeService stockPickingTypeService)
    {
        _stockPickingTypeService = stockPickingTypeService;
    }

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagingParam<SortStockPickingTypeCriteria> paginationModel)
    {
        var result = await _stockPickingTypeService.Get(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
