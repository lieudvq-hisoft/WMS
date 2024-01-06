using System;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Data.Models
{
	public class ProductModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? SalePrice { get; set; }
        public string? SerialNumber { get; set; }
        public string? InternalCode { get; set; }
        public List<string>? Images { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public int TotalInventory { get; set; }
    }

    public class ProductInventoryModel
    {
        public string? Name { get; set; }
        public string? SerialNumber { get; set; }
        public int totalInventory { get; set; }
    }

    public class InventoryFPModel
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public string SerialCode { get; set; }
        public bool IsAvailable { get; set; }
        public List<InventoryLocationFPModel> InventoryLocations { get; set; }

    }

    public class InventoryLocationFPModel
    {
        public LocationFPModel Location { get; set; }
    }

    public class LocationFPModel
    {
        public Guid Id { get; set; }
        public RackLevelModel RackLevel { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int SectionNumber { get; set; }
    }

    public class ProductCreateModel
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? SalePrice { get; set; }
        public string? SerialNumber { get; set; }
        public string? InternalCode { get; set; }
    }

    public class ProductUpdateModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public double? SalePrice { get; set; }
        public string? SerialNumber { get; set; }
        public string? InternalCode { get; set; }
    }

    public class ProductSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }

    public class UploadImgModel
    {
        public Guid Id { get; set; }
        public IFormFile File { get; set; }
    }

    public class DeleteImgModel
    {
        public Guid Id { get; set; }
        public string Path { get; set; }
    }

    public class ProductCompletedModel : ProductModel
    {
        public List<InventoryFPModel>? Inventories { get; set; }

    }
}

