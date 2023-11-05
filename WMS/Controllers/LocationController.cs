﻿using Data.Common.PaginationModel;
using Data.Enums;
using Data.Model;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace WMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] LocationCreateModel model)
        {
            var result = await _locationService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] LocationUpdateModel model)
        {
            var result = await _locationService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PagingParam<LocationSortCriteria> paginationModel, [FromQuery] LocationSearchModel searchModel)
        {
            var result = await _locationService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _locationService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}
