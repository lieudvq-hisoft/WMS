using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace UomCategory.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class ProductTemplateAttributeLineController : ControllerBase
{
    private readonly IProductTemplateAttributeLineService _productTemplateAttributeLineService;
    public ProductTemplateAttributeLineController(IProductTemplateAttributeLineService productTemplateAttributeLineService)
    {
        _productTemplateAttributeLineService = productTemplateAttributeLineService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] ProductTemplateAttributeLineCreate model)
    {
        var result = await _productTemplateAttributeLineService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut]
    public async Task<ActionResult> Update([FromBody] ProductTemplateAttributeLineUpdate model)
    {
        var result = await _productTemplateAttributeLineService.Update(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _productTemplateAttributeLineService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
