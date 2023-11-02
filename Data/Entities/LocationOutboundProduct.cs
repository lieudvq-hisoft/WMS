using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class LocationOutboundProduct : BaseEntity
{
    public Guid OutboundProductId { get; set; }
    [ForeignKey("OutboundProductId")]
    public virtual OutboundProduct OutboundProduct { get; set; }

    public Guid LocationId { get; set; }
    [ForeignKey("LocationId")]
    public virtual Location Location { get; set; }
    public int Quantity { get; set; }
}
