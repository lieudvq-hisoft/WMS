using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class InboundProduct : BaseEntity
{
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    public Guid ReceiptId { get; set; }
    [ForeignKey("ReceiptId")]
    public virtual Receipt? Receipt { get; set; }

    public int Quantity { get; set; }
    public double? PurchaseUnitPrice { get; set; }
    public double? TotalCost { get; set; }
    public DateTime? ManufacturedDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? Note { get; set; }
    public InboundProductStatus Status { get; set; } = InboundProductStatus.Pending;
    public int? BatchNumber { get; set; }
}
