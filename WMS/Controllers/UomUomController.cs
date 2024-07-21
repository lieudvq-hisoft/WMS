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
public class UomUomController : ControllerBase
{
    private readonly IUomUomService _uomUomService;
    public UomUomController(IUomUomService uomUomService)
    {
        _uomUomService = uomUomService;
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UomUomCreate model)
    {
        var result = await _uomUomService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Type")]
    public async Task<ActionResult> UpdateType([FromBody] UomUomUpdateType model)
    {
        var result = await _uomUomService.UpdateType(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Factor")]
    public async Task<ActionResult> UpdateFactor([FromBody] UomUomUpdateFactor model)
    {
        var result = await _uomUomService.UpdateFactor(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Info")]
    public async Task<ActionResult> UpdateInfo([FromBody] UomUomUpdate model)
    {
        var result = await _uomUomService.UpdateInfo(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _uomUomService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Select")]
    public async Task<ActionResult> GetForSelect()
    {
        var result = await _uomUomService.GetForSelect();
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
