using Data.Entities;
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

    public class StockLocationInfo : StockLocationModel
    {
        public StockLocationModel? ParentLocation { get; set; }

    }

    public class StockLocationUpdate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public LocationType? Usage { get; set; }
    }

    public class StockLocationCreate
    {
        public string Name { get; set; }
        public Guid? LocationId { get; set; }
        public LocationType Usage { get; set; }
    }

    public class StockLocationParentUpdate
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
    }
}

