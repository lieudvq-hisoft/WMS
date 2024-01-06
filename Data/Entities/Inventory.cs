using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
    public string? Note { get; set; }
    public string? Description { get; set; }
    public int QuantityOnHand { get; set; } = 0;
    public string SerialCode { get; set; }
    public bool IsAvailable { get; set; } = true;
    public virtual ICollection<InventoryLocation> InventoryLocations { get; set; }
}
