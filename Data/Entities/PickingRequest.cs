using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class PickingRequest : BaseEntity
{
    public Guid OrderId { get; set; }
    [ForeignKey("OrderId")]
    public virtual Order? Order { get; set; }

    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    public string? Note { get; set; }
    public int Quantity { get; set; }
    public PickingRequestStatus Status { get; set; } = PickingRequestStatus.Pending;
    public PickingRequestType Type { get; set; } = PickingRequestType.Outbound;
    public virtual ICollection<PickingRequestInventory> PickingRequestInventories { get; set; }
    public virtual ICollection<PickingRequestUser> PickingRequestUsers { get; set; }

}
