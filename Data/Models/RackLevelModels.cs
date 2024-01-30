using Data.Entities;

namespace Data.Models
{
	public class RackLevelModel
    {
        public Guid Id { get; set; }
        public RackModel Rack { get; set; }
        public int LevelNumber { get; set; }
        public string? Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class RackLevelCreateModel
    {
        public Guid RackId { get; set; }
        public int LevelNumber { get; set; }
        public string? Description { get; set; }
    }

    public class RackLevelUpdateModel
    {
        public Guid Id { get; set; }
        public int LevelNumber { get; set; }
        public string? Description { get; set; }
    }

    public class RackLevelSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

