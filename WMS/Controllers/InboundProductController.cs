using Data.Common.PaginationModel;
using Data.Enums;
using Data.Model;
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

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok();
        }
    }
}

