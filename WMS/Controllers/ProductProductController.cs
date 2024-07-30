using Data.Common.PaginationModel;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace UomCategory.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ProductProductController : ControllerBase
{
    private readonly IProductProductService _productProductService;
    public ProductProductController(IProductProductService productProductService)
    {
        _productProductService = productProductService;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _productProductService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("StockQuant/{id}")]
    public async Task<ActionResult> GetStockQuant([FromQuery] PagingParam<StockQuantSortCriteria> paginationModel, Guid id)
    {
        var result = await _productProductService.GetStockQuant(paginationModel, id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

}