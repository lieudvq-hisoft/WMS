using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Product : BaseEntity
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? SalePrice { get; set; }
    public string? SerialNumber { get; set; }
    public string? InternalCode { get; set; }
    public string? Image { get; set; }
    public virtual ICollection<Inventory> Inventories { get; set; }
    public virtual ICollection<PickingRequest> PickingRequests { get; set; }
}
