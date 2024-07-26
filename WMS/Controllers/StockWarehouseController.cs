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
    public class StockWarehouseController : ControllerBase
	{
        private readonly IStockWarehouseService _stockWarehouseService;
        public StockWarehouseController(IStockWarehouseService stockWarehouseService)
        {
            _stockWarehouseService = stockWarehouseService;
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] StockWarehouseCreate model)
        {
            var result = await _stockWarehouseService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] StockWarehouseUpdate model)
        {
            var result = await _stockWarehouseService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PagingParam<StockWarehouseSortCriteria> paginationModel)
        {
            var result = await _stockWarehouseService.Get(paginationModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _stockWarehouseService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Info/{id}")]
        public async Task<ActionResult> GetInfo(Guid id)
        {
            var result = await _stockWarehouseService.GetInfo(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

