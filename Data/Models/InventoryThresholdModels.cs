using System;
namespace Data.Models
{
    public class InventoryThresholdModel
    {
        public Guid Id { get; set; }
        public int ThresholdQuantity { get; set; }
        public string CronExpression { get; set; }
    }

    public class InventoryThresholdCreateModel
    {
        public int ThresholdQuantity { get; set; } = 2;
        public string CronExpression { get; set; } = "20 16 * * *";
    }

    public class InventoryThresholdUpdateModel
    {
        public Guid Id { get; set; }
        public int? ThresholdQuantity { get; set; }
        public string? CronExpression { get; set; }
    }
}

