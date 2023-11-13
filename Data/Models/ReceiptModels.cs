using Data.Entities;
using Data.Enums;
using Data.Model;

namespace Data.Models
{
	public class ReceiptModel
    {
        public Guid Id { get; set; }
        public Supplier Supplier { get; set; }
        public UserModel ReceivedByUser { get; set; }
        public ProductModel Product { get; set; }

        public int Quantity { get; set; }
        public string? Note { get; set; }
        public ReceiptStatus status { get; set; }
        public ReceiptType Type { get; set; }
        public double? CostPrice { get; set; }
    }

    public class ReceiptCreateModel
    {
        public Guid SupplierId { get; set; }
        public Guid ReceivedBy { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Note { get; set; }
        public double? CostPrice { get; set; }
    }

    public class ReceiptUpdateModel
    {
        public Guid Id { get; set; }
        public int? Quantity { get; set; }
        public string? Note { get; set; }
        public double? CostPrice { get; set; }
    }

    public class ReceiptCompleteModel
    {
        public Guid Id { get; set; }
        public Guid LocationId { get; set; }
    }

    public class ReceiptSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

