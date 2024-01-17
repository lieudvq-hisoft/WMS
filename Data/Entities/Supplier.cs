using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class Supplier : BaseEntity
{
    public string? Name { get; set; }
    public string? StaffName { get; set; }
    public string? Position { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public int? Status { get; set; }
}
