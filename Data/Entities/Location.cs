using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Location : BaseEntity
{
    public Guid RackLevelId { get; set; }
    [ForeignKey("RackLevelId")]
    public virtual RackLevel RackLevel { get; set; }

    public string Name { get; set; }
    public string? Description { get; set; }
    public int SectionNumber { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; }
}
