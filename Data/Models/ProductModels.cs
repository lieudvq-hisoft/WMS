using System;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;

namespace Data.Models
{
	public class ProductModel
    {
        public Guid Id { get; set; }
        public Supplier Supplier { get; set; }

        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? UnitPrice { get; set; }
        public double? CostPrice { get; set; }
        public int? InventoryCount { get; set; }
        public string? Image { get; set; }
        public int? Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class ProductCreateModel
    {
        public Guid SupplierId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? UnitPrice { get; set; }
        public double? CostPrice { get; set; }
        public int? InventoryCount { get; set; }
        public int? Status { get; set; }
    }

    public class ProductUpdateModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? UnitPrice { get; set; }
        public double? CostPrice { get; set; }
        public int? InventoryCount { get; set; }
        public int? Status { get; set; }
    }

    public class ProductSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

