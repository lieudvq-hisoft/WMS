using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Supplier : BaseEntity
{
    public string? Name { get; set; }
    public string? Address { get; set; }
    public double? Phone { get; set; }
    public double? Email { get; set; }
    public int? Status { get; set; }
}
