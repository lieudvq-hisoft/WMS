using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class ReceiptInventory : BaseEntity
{
    public Guid ReceiptId { get; set; }
    [ForeignKey("ReceiptId")]
    public virtual Receipt? Receipt { get; set; }

    public Guid InventoryId { get; set; }
    [ForeignKey("InventoryId")]
    public virtual Inventory? Inventory { get; set; }

    public int Quantity { get; set; }
    public string? Note { get; set; }
}
