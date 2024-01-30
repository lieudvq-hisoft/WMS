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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpGet("Barcode/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetBarcode(Guid id)
        {
            var result = await _inventoryService.GetBarcode(id);
            if (result.Succeed) return File((byte[])result.Data, "image/png");
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Detail/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetDetail(Guid id)
        {
            var result = await _inventoryService.GetDetail(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut("Location")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateLocation(UpdateLocationModel model)
        {
            var result = await _inventoryService.UpdateLocation(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(InventoryUpdateModel model)
        {
            var result = await _inventoryService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

