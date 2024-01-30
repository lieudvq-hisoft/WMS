using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class PickingRequestInventory : BaseEntity
{
    public Guid PickingRequestId { get; set; }
    [ForeignKey("PickingRequestId")]
    public virtual PickingRequest PickingRequest { get; set; }

    public Guid InventoryId { get; set; }
    [ForeignKey("InventoryId")]
    public virtual Inventory Inventory { get; set; }
    public int Quantity { get; set; }
}
