
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
public class UomCategoryController : ControllerBase
{
    private readonly IUomCategoryService _uomCategoryService;
    public UomCategoryController(IUomCategoryService uomCategoryService)
    {
        _uomCategoryService = uomCategoryService;
    }

    [HttpGet]
    public async Task<ActionResult> Get([FromQuery] PagingParam<SortCriteria> paginationModel)
    {
        var result = await _uomCategoryService.Get(paginationModel);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpGet("Info/{id}")]
    public async Task<ActionResult> GetInfo(Guid id)
    {
        var result = await _uomCategoryService.GetInfo(id);
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
    public async Task<ActionResult> Create([FromQuery] PagingParam<SortCriteria> paginationModel, Guid uomCateId)
    {
        var result = await _uomCategoryService.GetUomUom(paginationModel, uomCateId);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

    [HttpPut("Info")]
    public async Task<ActionResult> UpdateInfo([FromBody] UomCategoryUpdate model)
    {
        var result = await _uomCategoryService.UpdateInfo(model);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _uomCategoryService.Delete(id);
        if (result.Succeed) return Ok(result.Data);
        return BadRequest(result.ErrorMessage);
    }

}
