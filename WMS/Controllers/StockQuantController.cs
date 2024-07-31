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
    public class StockQuantController : ControllerBase
	{
        private readonly IStockQuantService _stockQuantService;
        public StockQuantController(IStockQuantService stockQuantService)
        {
            _stockQuantService = stockQuantService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] StockQuantCreate model)
        {
            var result = await _stockQuantService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("MoveLines/{id}")]
        public async Task<ActionResult> GetMoveLines([FromQuery] PagingParam<StockMoveLineSortCriteria> paginationModel, Guid id)
        {
            var result = await _stockQuantService.GetStockMoveLines(paginationModel, id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut()]
        public async Task<ActionResult> Update([FromBody] StockQuantUpdate model)
        {
            var result = await _stockQuantService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Set/{id}")]
        public async Task<ActionResult> SetStockQuant(Guid id)
        {
            var result = await _stockQuantService.SetStockQuant(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Clear/{id}")]
        public async Task<ActionResult> ClearStockQuant(Guid id)
        {
            var result = await _stockQuantService.ClearStockQuant(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Apply/{id}")]
        public async Task<ActionResult> ApplyStockQuant(Guid id)
        {
            var result = await _stockQuantService.ApplyStockQuant(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

