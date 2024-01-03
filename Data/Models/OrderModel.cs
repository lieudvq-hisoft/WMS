using System;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;
using Data.Enums;
using Microsoft.AspNetCore.Http;

namespace Data.Models
{
    public class OrderInnerModel
    {
        public Guid Id { get; set; }
        public Guid SentBy { get; set; }
        public User? SentByUser { get; set; }
        public string? Note { get; set; }
        public OrderStatus Status { get; set; }
        public List<string>? Files { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class OrderModel : OrderInnerModel
    {
        public List<PickingRequestModel> PickingRequests { get; set; }
    }

    public class OrderCreateModel
    {
        public string? Note { get; set; }
        public List<PickingRequestInnerCreateModel>? PickingRequests { get; set; }
    }

}

