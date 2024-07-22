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
public class ProductAttributeController : ControllerBase
{
    private readonly IProductAttributeService _productAttributeService;
    public ProductAttributeController(IProductAttributeService productAttributeService)
    {
        _productAttributeService = productAttributeService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductAttributeCreate model)
    {
        var result = await _productAttributeService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] ProductAttributeUpdate model)
    {
        var result = await _productAttributeService.Update(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet()]
    public async Task<ActionResult> Get([FromQuery] PagingParam<SortCriteria> paginationModel)
    {
        var result = await _productAttributeService.Get(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Value/{id}")]
    public async Task<ActionResult> GetAttributeValue([FromQuery] PagingParam<SortCriteria> paginationModel, Guid id)
    {
        var result = await _productAttributeService.GetAttributeValue(paginationModel, id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _productAttributeService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Info/{id}")]
    public async Task<ActionResult> GetInfo(Guid id)
    {
        var result = await _productAttributeService.GetInfo(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Select")]
    public async Task<ActionResult> GetForSelect()
    {
        var result = await _productAttributeService.GetForSelect();
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
