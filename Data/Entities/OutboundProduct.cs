using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class OutboundProduct : BaseEntity
{
    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    public Guid PickingId { get; set; }
    [ForeignKey("PickingId")]
    public virtual PickingRequest? PickingRequest { get; set; }

    public int Quantity { get; set; }
    public OutboundProductStatus Status { get; set; } = OutboundProductStatus.Pending;
}
