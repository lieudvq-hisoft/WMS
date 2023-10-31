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
    public class InboundProductController : ControllerBase
    {
        private readonly IInboundProductService _inboundProductService;
        public InboundProductController(IInboundProductService inboundProductService)
        {
            _inboundProductService = inboundProductService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] InboundProductCreateModel model)
        {
            var result = await _inboundProductService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] InboundProductUpdateModel model)
        {
            var result = await _inboundProductService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PagingParam<InboundProductSortCriteria> paginationModel, [FromQuery] InboundProductSearchModel searchModel)
        {
            var result = await _inboundProductService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _inboundProductService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

