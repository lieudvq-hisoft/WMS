using Data.Entities;

namespace Data.Models
{
	public class SupplierModel : Supplier
	{
	}

    public class SupplierCreateModel
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? Status { get; set; }
    }

    public class SupplierUpdateModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int? Status { get; set; }
    }

    public class SupplierSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

