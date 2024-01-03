using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;

namespace Data.Entities;

public class Order : BaseEntity
{
    public Guid SentBy { get; set; }
    [ForeignKey("SentBy")]
    public virtual User? SentByUser { get; set; }
    public string? Note { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public List<string>? Files { get; set; }
}
