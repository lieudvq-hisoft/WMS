using Data.Entities;

namespace Data.Models
{
	public class RackModel
    {
        public Guid Id { get; set; }
        public int RackNumber { get; set; }
        public string? Description { get; set; }
        public int TotalLevel { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class RackCreateModel
    {
        public int RackNumber { get; set; }
        public string? Description { get; set; }
        public int TotalLevel { get; set; }
    }

    public class RackUpdateModel
    {
        public Guid Id { get; set; }
        public int? RackNumber { get; set; }
        public string? Description { get; set; }
        public int? TotalLevel { get; set; }
    }

    public class RackSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

