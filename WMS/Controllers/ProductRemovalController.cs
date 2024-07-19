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
public class ProductRemovalController : ControllerBase
{
    private readonly IProductRemovalService _productRemovalService;
    public ProductRemovalController(IProductRemovalService productRemovalService)
    {
        _productRemovalService = productRemovalService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductRemovalCreate model)
    {
        var result = await _productRemovalService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] ProductRemovalUpdate model)
    {
        var result = await _productRemovalService.Update(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagingParam<SortCriteria> paginationModel)
    {
        var result = await _productRemovalService.Get(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _productRemovalService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Select")]
    public async Task<ActionResult> GetForSelect()
    {
        var result = await _productRemovalService.GetForSelect();
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
