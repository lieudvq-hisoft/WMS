using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class InventoryLocation : BaseEntity
{
    public Guid InventoryId { get; set; }
    [ForeignKey("InventoryId")]
    public virtual Inventory? Inventory { get; set; }

    public Guid LocationId { get; set; }
    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }
}
