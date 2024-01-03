using Confluent.Kafka;
using Data.Common.PaginationModel;
using Data.Enums;
using Data.Model;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Core;
using Services.Utils;

namespace WMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<ActionResult> Get([FromQuery] PagingParam<OrderSortCriteria> paginationModel, [FromQuery] OrderSearchModel searchModel)
        {
            var result = await _orderService.Get(paginationModel, searchModel);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult> Create([FromBody] OrderCreateModel model)
        {
            var result = await _orderService.Create(model, Guid.Parse(User.GetId()));
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPut]
        //[Authorize(Roles = "Manager")]
        public async Task<ActionResult> Update([FromBody] OrderUpdateModel model)
        {
            var result = await _orderService.Update(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("File/Upload")]
        public async Task<ActionResult> UploadFile([FromForm] UploadFileModel model)
        {
            var result = await _orderService.UploadFile(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("File")]
        public async Task<ActionResult> DeleteFile([FromBody] FileModel model)
        {
            var result = await _orderService.DeleteFile(model);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

        [HttpPost("File/Download")]
        public async Task<ActionResult> DownloadFile([FromBody] FileModel model)
        {
            var result = await _orderService.DownloadFile(model);
            FileEModel file = (FileEModel)result.Data;
            if (result.Succeed) return File(file.Content, "application/octet-stream", "order"+file.Extension);
            return BadRequest(result.ErrorMessage);
        }

        [HttpDelete("id")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _orderService.Delete(id);
            if (result.Succeed) return Ok(result.Data);
            return BadRequest(result.ErrorMessage);
        }

    }
}

