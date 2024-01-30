using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class PickingRequestUser : BaseEntity
{
    public Guid PickingRequestId { get; set; }
    [ForeignKey("PickingRequestId")]
    public virtual PickingRequest PickingRequest { get; set; }

    public Guid ReceivedBy { get; set; }
    [ForeignKey("ReceivedBy")]
    public virtual User ReceivedByUser { get; set; }
}
