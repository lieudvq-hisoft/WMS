using Data.Entities;

namespace Data.Models
{
	public class LocationModel
    {
        public Guid Id { get; set; }
        public RackLevelModel RackLevel { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int SectionNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public List<InventoryLocationPrivateModel> InventoryLocations { get; set; }
    }

    public class InventoryPrivateModel
    {
        public Guid Id { get; set; }
        public string Note { get; set; }
        public int QuantityOnHand { get; set; } = 0;
        public ProductModel Product { get; set; }

    }

    public class InventoryLocationPrivateModel
    {
        public InventoryPrivateModel Inventory { get; set; }
    }

    public class LocationCreateModel
    {
        public Guid RackLevelId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int SectionNumber { get; set; }
    }

    public class LocationUpdateModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? SectionNumber { get; set; }
    }

    public class LocationSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

