namespace Data.Entities;

public class Rack : BaseEntity
{
    public int RackNumber { get; set; }
    public string? Description { get; set; }
    public int TotalLevel { get; set; }
    public virtual ICollection<RackLevel> RackLevels { get; set; }
}
