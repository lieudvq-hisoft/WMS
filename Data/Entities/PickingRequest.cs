using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class PickingRequest : BaseEntity
{
    public Guid SentBy { get; set; }
    [ForeignKey("SentBy")]
    public virtual User? SentByUser { get; set; }

    public Guid ProductId { get; set; }
    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }

    public string? Note { get; set; }
    public int Quantity { get; set; }
    public PickingRequestStatus Status { get; set; } = PickingRequestStatus.Pending;
    public virtual ICollection<PickingRequestInventory> PickingRequestInventories { get; set; }
}
