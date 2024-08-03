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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _stockMoveService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Quantity")]
        public async Task<ActionResult> UpdateQuantity([FromBody] StockMoveQuantityUpdate model)
        {
            var result = await _stockMoveService.UpdateQuantity(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

