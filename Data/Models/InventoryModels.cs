using Data.Entities;
using Data.Model;

namespace Data.Models
{
	public class InventoryModel
	{
        public Guid Id { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public int QuantityOnHand { get; set; } = 0;
        public string SerialCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public List<InventoryLocationModel> InventoryLocations { get; set; }
    }

    public class InventoryInnerModel
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public int QuantityOnHand { get; set; } = 0;
        public string SerialCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public List<InventoryLocationInnerModel>? InventoryLocations { get; set; }
    }

    public class InventoryLocationInnerModel
    {
        public LocationInnerModel? Location { get; set; }
    }

    public class InventoryLocationModel
    {
        public LocationModel? Location { get; set; }
    }

    public class InventoryFIModel
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
        public int QuantityOnHand { get; set; } = 0;
        public string SerialCode { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public ProductModel Product { get; set; }
        public List<InventoryLocationFIModel> InventoryLocations { get; set; }
    }

    public class InventoryUpdateModel
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public string Description { get; set; }
    }

    public class InventoryLocationFIModel
    {
        public LocationFIModel Location { get; set; }
    }

    public class LocationFIModel
    {
        public Guid Id { get; set; }
        public RackLevelModel RackLevel { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int SectionNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class UpdateLocationModel
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
    }

}

