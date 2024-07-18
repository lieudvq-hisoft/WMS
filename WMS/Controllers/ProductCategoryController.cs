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
public class ProductCategoryController : ControllerBase
{
    private readonly IProductCategoryService _productCategoryService;
    public ProductCategoryController(IProductCategoryService productCategoryService)
    {
        _productCategoryService = productCategoryService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductCategoryCreate model)
    {
        var result = await _productCategoryService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] ProductCategoryUpdate model)
    {
        var result = await _productCategoryService.Update(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Parent")]
    public async Task<ActionResult> UpdateParent([FromBody] ProductCategoryParentUpdate model)
    {
        var result = await _productCategoryService.UpdateParent(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagingParam<ProductCategorySortCriteria> paginationModel)
    {
        var result = await _productCategoryService.Get(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("SelectParent/{id}")]
    public async Task<ActionResult> GetForSelectParent(Guid id)
    {
        var result = await _productCategoryService.GetForSelectParent(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _productCategoryService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Info/{id}")]
    public async Task<ActionResult> GetInfo(Guid id)
    {
        var result = await _productCategoryService.GetInfo(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
