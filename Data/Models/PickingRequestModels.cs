using Data.Entities;
using Data.Enums;
using Data.Model;

namespace Data.Models
{
	public class PickingRequestModel
    {   
        public Guid Id { get; set; }
        public UserModel SentByUser { get; set; }
        public ProductModel Product { get; set; }
        public string? Note { get; set; }
        public int Quantity { get; set; }
        public PickingRequestStatus Status { get; set; }
        public PickingRequestType Type { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class PickingRequestCreateModel
    {
        public Guid SentBy { get; set; }
        public Guid ProductId { get; set; }
        public string? Note { get; set; }
        public int Quantity { get; set; }
    }

    public class PickingRequestUpdateModel
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
        public int? Quantity { get; set; }
    }

    public class PickingRequestInventory
    {
        public Guid InventoryId { get; set; }
        //public int Quantity { get; set; }
    }

    public class PickingRequestCompleteModel
    {
        public Guid Id { get; set; }
        public List<Guid> ListInventoryId { get; set; }
    }

    public class PickingRequestSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }

    public class DailyReportPickingRequest
    {
        public DateTime Date { get; set; }
        public int TotalCompleted { get; set; }
        public int TotalQuantityCompleted { get; set; }
        public int TotalPending { get; set; }
        public int TotalQuantityPending { get; set; }
    }
}

