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
public class ProductAttributeValueController : ControllerBase
{
    private readonly IProductAttributeValueService _productAttributeValueService;
    public ProductAttributeValueController(IProductAttributeValueService productAttributeValueService)
    {
        _productAttributeValueService = productAttributeValueService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductAttributeValueCreate model)
    {
        var result = await _productAttributeValueService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] ProductAttributeValueUpdate model)
    {
        var result = await _productAttributeValueService.Update(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _productAttributeValueService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
