using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class PickingRequestModel
    {   
        public Guid Id { get; set; }
        public User SentByUser { get; set; }
        public Product Product { get; set; }
        public string? Note { get; set; }
        public int Quantity { get; set; }
        public PickingRequestStatus Status { get; set; }
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
        public int Quantity { get; set; }
    }

    public class PickingRequestCompleteModel
    {
        public Guid Id { get; set; }
        public List<PickingRequestInventory> pickingRequestInventories { get; set; }
    }

    public class PickingRequestSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

