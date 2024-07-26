using Data.Enums;

namespace Data.Models
{
	public class StockLocationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? CompleteName { get; set; }
        public string? ParentPath { get; set; }
        public string? Barcode { get; set; }
        public LocationType Usage { get; set; }
    }
}

