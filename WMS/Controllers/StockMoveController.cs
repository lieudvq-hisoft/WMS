using Data.Common.PaginationModel;
using Data.Enums;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace WMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class StockMoveController : ControllerBase
	{
        private readonly IStockMoveService _stockMoveService;
        public StockMoveController(IStockMoveService stockMoveService)
        {
            _stockMoveService = stockMoveService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] StockMoveCreate model)
        {
            var result = await _stockMoveService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

