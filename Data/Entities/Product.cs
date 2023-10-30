using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Product : BaseEntity
{
    public Guid SupplierId { get; set; }
    [ForeignKey("SupplierId")]
    public virtual Supplier? Supplier { get; set; }

    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? UnitPrice { get; set; }
    public double? CostPrice { get; set; }
    public int? InventoryCount { get; set; }
    public string? Image { get; set; }
    public int? Status { get; set; }
}
