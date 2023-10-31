using System;
using Data.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
	public class InboundProductModel
    {
        public Guid Id { get; set; }
        public Product? Product { get; set; }
        public Receipt? Receipt { get; set; }
        public int? Quantity { get; set; }
        public double? PurchaseUnitPrice { get; set; }
        public double? TotalCost { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string? Note { get; set; }
        public int? Status { get; set; }
        public int? BatchNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class InboundProductCreateModel
    {
        public Guid ProductId { get; set; }
        public Guid ReceiptId { get; set; }
        public int? Quantity { get; set; }
        public double? PurchaseUnitPrice { get; set; }
        public double? TotalCost { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string? Note { get; set; }
        public int? Status { get; set; }
        public int? BatchNumber { get; set; }
    }

    public class InboundProductUpdateModel
    {
        public Guid Id { get; set; }
        public int? Quantity { get; set; }
        public double? PurchaseUnitPrice { get; set; }
        public double? TotalCost { get; set; }
        public DateTime? ManufacturedDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string? Note { get; set; }
        public int? Status { get; set; }
        public int? BatchNumber { get; set; }
    }

    public class InboundProductSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

