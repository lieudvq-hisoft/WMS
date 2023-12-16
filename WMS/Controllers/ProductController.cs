using Confluent.Kafka;
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
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Create([FromBody] ProductCreateModel model)
        {
            var result = await _productService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Update([FromBody] ProductUpdateModel model)
        {
            var result = await _productService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PagingParam<ProductSortCriteria> paginationModel, [FromQuery] ProductSearchModel searchModel)
        {
            var result = await _productService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Detail/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetDetail(Guid id)
        {
            var result = await _productService.GetDetail(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Qrcode/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetQrcode(Guid id)
        {
            var result = await _productService.GetQrcode(id);
            if (result.Succeed) return File((byte[])result.Data, "image/png");
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Barcode/{id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetBarcode(Guid id)
        {
            var result = await _productService.GetBarcode(id);
            if (result.Succeed) return File((byte[])result.Data, "image/png");
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("InventoryQuantity/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetInventoryQuantity(Guid id)
        {
            var result = await _productService.GetInventoryQuantity(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Inventories/{id}")]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetInventories(Guid id)
        {
            var result = await _productService.GetInventories(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("PickingRequest/Completed/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetPickingRequestCompleted(Guid id)
        {
            var result = await _productService.GetPickingRequestCompleted(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("PickingRequest/Pending/{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> GetPickingRequestPending(Guid id)
        {
            var result = await _productService.GetPickingRequestPending(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Manager")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _productService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("UploadImg")]
        public async Task<ActionResult> UploadImg([FromForm] UploadImgModel model)
        {
            var result = await _productService.UploadImg(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("DeleteImg")]
        public async Task<ActionResult> DeleteImg([FromForm] DeleteImgModel model)
        {
            var result = await _productService.DeleteImg(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Report/Inventory")]
        public async Task<ActionResult> GetReportInventory()
        {
            var result = await _productService.GetReportInventory();
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

