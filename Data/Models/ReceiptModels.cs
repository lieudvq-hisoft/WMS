using Data.Entities;
using Data.Model;

namespace Data.Models
{
	public class ReceiptModel
    {
        public Guid Id { get; set; }
        public Supplier Supplier { get; set; }
        public UserModel ReceivedByUser { get; set; }

        public int? ReceiptNumber { get; set; }
        public int? ReceiptType { get; set; }
        public double? TotalAmount { get; set; }
        public string? Note { get; set; }
        public int? InventoryCount { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int? Status { get; set; }
    }

    public class ReceiptCreateModel
    {
        public Guid SupplierId { get; set; }
        public Guid ReceivedBy { get; set; }

        public int? ReceiptNumber { get; set; }
        public int? ReceiptType { get; set; }
        public double? TotalAmount { get; set; }
        public string? Note { get; set; }
        public int? InventoryCount { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int? Status { get; set; }
    }

    public class ReceiptUpdateModel
    {
        public Guid Id { get; set; }
        public int? ReceiptNumber { get; set; }
        public int? ReceiptType { get; set; }
        public double? TotalAmount { get; set; }
        public string? Note { get; set; }
        public int? InventoryCount { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public int? Status { get; set; }
    }

    public class ReceiptSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

