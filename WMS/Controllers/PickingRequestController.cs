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
    public class PickingRequestController : ControllerBase
    {
        private readonly IPickingRequestService _pickingRequestService;
        public PickingRequestController(IPickingRequestService pickingRequestService)
        {
            _pickingRequestService = pickingRequestService;
        }

        [HttpPost]
        //[Authorize(Roles = "Manager,Staff")]
        public async Task<ActionResult> Create([FromBody] PickingRequestCreateModel model)
        {
            var result = await _pickingRequestService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("Complete")]
        //[Authorize(Roles = "Manager,Staff")]
        public async Task<ActionResult> Complete([FromBody] PickingRequestCompleteModel model)
        {
            var result = await _pickingRequestService.Complete(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<ActionResult> Update([FromBody] PickingRequestUpdateModel model)
        {
            var result = await _pickingRequestService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        //[Authorize(Roles = "Manager,Staff")]
        public async Task<ActionResult> Get([FromQuery] PagingParam<PickingRequestSortCriteria> paginationModel, [FromQuery] PickingRequestSearchModel searchModel)
        {
            var result = await _pickingRequestService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Manager,Staff")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _pickingRequestService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("WeeklyReport")]
        //[Authorize(Roles = "Staff")]
        public async Task<ActionResult> GetWeeklyReport()
        {
            var result = await _pickingRequestService.GetWeeklyReport();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

