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
        public async Task<ActionResult> Create([FromBody] PickingRequestCreateModel model)
        {
            var result = await _pickingRequestService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] PickingRequestUpdateModel model)
        {
            var result = await _pickingRequestService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PagingParam<PickingRequestSortCriteria> paginationModel, [FromQuery] PickingRequestSearchModel searchModel)
        {
            var result = await _pickingRequestService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _pickingRequestService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

