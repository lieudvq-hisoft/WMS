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
    public class RackLevelController : ControllerBase
    {
        private readonly IRackLevelService _rackLevelService;
        public RackLevelController(IRackLevelService rackLevelService)
        {
            _rackLevelService = rackLevelService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Create([FromBody] RackLevelCreateModel model)
        {
            var result = await _rackLevelService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Update([FromBody] RackLevelUpdateModel model)
        {
            var result = await _rackLevelService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PagingParam<RackLevelSortCriteria> paginationModel, [FromQuery] RackLevelSearchModel searchModel)
        {
            var result = await _rackLevelService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _rackLevelService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

