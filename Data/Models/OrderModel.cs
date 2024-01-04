using System;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;
using Data.Enums;
using Data.Model;
using Microsoft.AspNetCore.Http;

namespace Data.Models
{
    public class OrderInnerModel
    {
        public Guid Id { get; set; }
        public Guid SentBy { get; set; }
        public UserModel? SentByUser { get; set; }
        public string? Note { get; set; }
        public OrderStatus Status { get; set; }
        public List<string>? Files { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class OrderModel : OrderInnerModel
    {
        public List<PickingRequestInnerOrderModel> PickingRequests { get; set; }
    }

    public class OrderSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }

    public class OrderCreateModel
    {
        public string? Note { get; set; }
        public List<PickingRequestInnerCreateModel>? PickingRequests { get; set; }
    }

    public class OrderUpdateModel
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
    }

    public class UploadFileModel
    {
        public Guid Id { get; set; }
        public IFormFile File { get; set; }
    }

    public class FileModel
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
    }

    public class OrderCompleteModel
    {
        public Guid Id { get; set; }
    }

}

