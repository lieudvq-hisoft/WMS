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
    public class RackController : ControllerBase
    {
        private readonly IRackService _rackService;
        public RackController(IRackService rackService)
        {
            _rackService = rackService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] RackCreateModel model)
        {
            var result = await _rackService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] RackUpdateModel model)
        {
            var result = await _rackService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PagingParam<RackSortCriteria> paginationModel, [FromQuery] RackSearchModel searchModel)
        {
            var result = await _rackService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _rackService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

