using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;
using Data.Enums;
using Data.Model;

namespace Data.Models
{
	public class PickingRequestModel
    {
        public Guid Id { get; set; }
        public User SentByUser { get; set; }
        public string? Note { get; set; }
        public PickingRequestStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class PickingRequestCreateModel
    {
        public Guid SentBy { get; set; }
        public string? Note { get; set; }
    }

    public class PickingRequestUpdateModel
    {
        public Guid Id { get; set; }
        public string? Note { get; set; }
    }

    public class PickingRequestSearchModel
    {
        public string? SearchValue { get; set; } = "";
    }
}

