using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    //public Guid LocationId { get; set; }
    //[ForeignKey("LocationId")]
    //public virtual Location? Location { get; set; }
    public string? Note { get; set; }
    public int QuantityOnHand { get; set; } = 0;
    public virtual ICollection<InventoryLocation> InventoryLocations { get; set; }
}
