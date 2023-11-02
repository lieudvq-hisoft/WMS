using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class Receipt : BaseEntity
{
    public Guid SupplierId { get; set; }
    [ForeignKey("SupplierId")]
    public virtual Supplier? Supplier { get; set; }

    public Guid ReceivedBy { get; set; }
    [ForeignKey("ReceivedBy")]
    public virtual User? ReceivedByUser { get; set; }

    public int? ReceiptNumber { get; set; }
    public int? ReceiptType { get; set; }
    public double? TotalAmount { get; set; }
    public string? Note { get; set; }
    public int? InventoryCount { get; set; }
    public DateTime? ReceivedDate { get; set; }
    public ReceiptStatus Status { get; set; } = ReceiptStatus.Pending;
}
