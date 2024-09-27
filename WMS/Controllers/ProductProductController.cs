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

    [HttpGet("ProductVariant")]
    public async Task<ActionResult> GetProductVariant()
    {
        var result = await _productProductService.GetProductVariant();
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
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

    [HttpGet("UomUom/Select/{id}")]
    public async Task<ActionResult> GetUomUomForSelect(Guid id)
    {
        var result = await _productProductService.GetUomUomForSelect(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("QrCode/{id}")]
    public async Task<ActionResult> GetQrcode(Guid id)
    {
        var result = await _productProductService.GetQrcode(id);
        if (result.Succeed) return File((byte[])result.Data, "image/png");
        return BadRequest(result.ErrorMessage);
    }

}