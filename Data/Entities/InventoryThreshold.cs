namespace Data.Entities;

public class InventoryThreshold : BaseEntity
{
    public int ThresholdQuantity { get; set; } = 2;
    public string CronExpression { get; set; } = "20 16 * * *";
}
