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
        public async Task<ActionResult> Create([FromBody] ProductCreateModel model)
        {
            var result = await _productService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
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

        [HttpGet("InventoryQuantity/{id}")]
        public async Task<ActionResult> GetInventoryQuantity(Guid id)
        {
            var result = await _productService.GetInventoryQuantity(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Inventories/{id}")]
        public async Task<ActionResult> GetInventories(Guid id)
        {
            var result = await _productService.GetInventories(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("PickingRequest/Completed/{id}")]
        public async Task<ActionResult> GetPickingRequestCompleted(Guid id)
        {
            var result = await _productService.GetPickingRequestCompleted(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("PickingRequest/Pending/{id}")]
        public async Task<ActionResult> GetPickingRequestPending(Guid id)
        {
            var result = await _productService.GetPickingRequestPending(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _productService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

