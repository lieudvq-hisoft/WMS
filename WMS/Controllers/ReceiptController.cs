using System;
using Data.Common.PaginationModel;
using Data.Enums;
using Data.Models;
using Data.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;

namespace WMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ReceiptController : ControllerBase
	{
        private readonly IReceiptService _receiptService;
        public ReceiptController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpPost]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Create([FromBody] ReceiptCreateModel model)
        {
            var result = await _receiptService.Create(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("Complete")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Complete([FromBody] ReceiptCompleteModel model)
        {
            var result = await _receiptService.Complete(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Update([FromBody] ReceiptUpdateModel model)
        {
            var result = await _receiptService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Get([FromQuery] PagingParam<ReceiptSortCriteria> paginationModel, [FromQuery] ReceiptSearchModel searchModel)
        {
            var result = await _receiptService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpGet("Pending")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<ActionResult> GetReceiptPending([FromQuery] PagingParam<ReceiptSortCriteria> paginationModel, [FromQuery] ReceiptSearchModel searchModel)
        {
            var result = await _receiptService.GetReceiptPending(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        [Authorize(Roles = "Staff")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _receiptService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }
    }
}

