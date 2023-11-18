using Data.Common.PaginationModel;
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
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Create([FromBody] LocationCreateModel model)
        {
            var result = await _locationService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
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

        [HttpGet("Barcode/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetBarcode(Guid id)
        {
            var result = await _locationService.GetBarcode(id);
            if (result.Succeed) return File((byte[])result.Data, "image/png");
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Detail/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetDetail(Guid id)
        {
            var result = await _locationService.GetDetail(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _locationService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

