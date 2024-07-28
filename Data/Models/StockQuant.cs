using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class StockQuantModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }
        public Guid? LotId { get; set; }
        public DateTime? InventoryDate { get; set; }
        public int Quantity { get; set; }
        public int? InventoryQuantity { get; set; }
        public int? InventoryDiffQuantity { get; set; }
        public bool? InventoryQuantitySet { get; set; }
    }

    public class StockQuantCreate
    {
        public Guid ProductId { get; set; }
        public Guid LocationId { get; set; }
        public Guid? LotId { get; set; }
        public int Quantity { get; set; }
    }

    public class StockQuantInfo
    {
        public Guid Id { get; set; }
        public ProductProductModel ProductProduct { get; set; }
        public StockLocationModel StockLocation { get; set; }
        public DateTime? InventoryDate { get; set; }
        public int Quantity { get; set; }
        public int? InventoryQuantity { get; set; }
        public int? InventoryDiffQuantity { get; set; }
        public bool? InventoryQuantitySet { get; set; }
        public string? UomUom { get; set; }
    }
}

