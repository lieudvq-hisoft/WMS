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
public class ProductTemplateController : ControllerBase
{
    private readonly IProductTemplateService _productTemplateService;
    public ProductTemplateController(IProductTemplateService productTemplateService)
    {
        _productTemplateService = productTemplateService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductTemplateCreate model)
    {
        var result = await _productTemplateService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] ProductTemplateUpdate model)
    {
        var result = await _productTemplateService.Update(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagingParam<SortCriteria> paginationModel)
    {
        var result = await _productTemplateService.Get(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _productTemplateService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Info/{id}")]
    public async Task<ActionResult> GetInfo(Guid id)
    {
        var result = await _productTemplateService.GetInfo(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("AttributeLine/{id}")]
    public async Task<ActionResult> GetAttributeLine([FromQuery] PagingParam<ProductTemplateAttributeLineSortCriteria> paginationModel, Guid id)
    {
        var result = await _productTemplateService.GetAttributeLine(paginationModel, id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("SuggestProductVariants/{id}")]
    public async Task<ActionResult> SuggestProductVariants(Guid id)
    {
        var result = await _productTemplateService.SuggestProductVariants(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost("ProductVariant")]
    public async Task<ActionResult> CreateProductVariant(ProductVariantCreate model)
    {
        var result = await _productTemplateService.CreateProductVariant(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("ProductVariant/{id}")]
    public async Task<ActionResult> GetProductVariant([FromQuery] PagingParam<ProductVariantSortCriteria> paginationModel, Guid id)
    {
        var result = await _productTemplateService.GetProductVariant(paginationModel, id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}