using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class Receipt : BaseEntity
{
    public Guid SupplierId { get; set; }
    [ForeignKey("SupplierId")]
    public virtual Supplier Supplier { get; set; }

    public Guid ReceivedBy { get; set; }
    [ForeignKey("ReceivedBy")]
    public virtual User ReceivedByUser { get; set; }

    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }

    public int Quantity { get; set; }
    public string? Note { get; set; }
    public ReceiptStatus Status { get; set; } = ReceiptStatus.Pending;
    public double? PurchaseUnitPrice { get; set; }
}