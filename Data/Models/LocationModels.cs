namespace Data.Models
{
	public class LocationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class LocationCreateModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }

    public class LocationUpdateModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class LocationSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

