using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class InboundProduct : BaseEntity
{
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    public Guid ReceivedId { get; set; }
    [ForeignKey("ReceivedId")]
    public virtual Receipt? Receipt { get; set; }

    public int? Quantity { get; set; }
    public double? PurchaseUnitPrice { get; set; }
    public double? TotalCost { get; set; }
    public DateTime? ManufacturedDate { get; set; }
    public DateTime? ExpiredDate { get; set; }
    public string? Note { get; set; }
    public int? Status { get; set; }
    public int? BatchNumber { get; set; }
}
