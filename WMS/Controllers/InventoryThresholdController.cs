using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace WMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class InventoryThresholdController : ControllerBase
    {
        private readonly IInventoryThresholdService _inventoryThresholdService;
        public InventoryThresholdController(IInventoryThresholdService inventoryThresholdService)
        {
            _inventoryThresholdService = inventoryThresholdService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _inventoryThresholdService.Get();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        //[HttpPost]
        //public async Task<ActionResult> Create([FromBody] InventoryThresholdCreateModel model)
        //{
        //    var result = await _inventoryThresholdService.Create(model);
        //    if (result.Succeed) return Ok(result.Data);
        //    return BadRequest(result.ErrorMessage);
        //}

        [HttpPut]
        public async Task<ActionResult> Update([FromBody] InventoryThresholdUpdateModel model)
        {
            var result = await _inventoryThresholdService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

    }
}

