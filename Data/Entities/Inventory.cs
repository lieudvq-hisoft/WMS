using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class Inventory : BaseEntity
{
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    public Guid LocationId { get; set; }
    [ForeignKey("LocationId")]
    public virtual Location? Location { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public int QuantityOnHand { get; set; }
    public InventoryType? Type { get; set; }
}
