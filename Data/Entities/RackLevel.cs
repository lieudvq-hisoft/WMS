using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class RackLevel : BaseEntity
{
    public Guid RackId { get; set; }
    [ForeignKey("RackId")]
    public virtual Rack Rack { get; set; }

    public int LevelNumber { get; set; }
    public string? Description { get; set; }

    public virtual ICollection<Location> Locations { get; set; }
}
