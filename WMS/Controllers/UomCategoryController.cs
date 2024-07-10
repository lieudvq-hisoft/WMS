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
public class UomCategoryController : ControllerBase
{
    private readonly IUomCategoryService _uomCategoryService;
    public UomCategoryController(IUomCategoryService uomCategoryService)
    {
        _uomCategoryService = uomCategoryService;
    }

    [HttpGet]
    public async Task<ActionResult> Get()
    {
        var result = await _uomCategoryService.Get();
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] UomCategoryCreate model)
    {
        var result = await _uomCategoryService.Create(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("GetUomUom/{uomCateId}")]
    public async Task<ActionResult> Create(Guid uomCateId)
    {
        var result = await _uomCategoryService.GetUomUom(uomCateId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
}
