﻿using System.ComponentModel.DataAnnotations;
using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class StockPickingModel
    {
        public Guid Id { get; set; }
        public StockLocationModel Location { get; set; }
        public StockLocationModel LocationDest { get; set; }
        public StockPickingTypeModel PickingType { get; set; }
        public string Name { get; set; }
        public PickingState? State { get; set; }
        public string? Note { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? DateDeadline { get; set; }
        public DateTime? DateDone { get; set; }
    }

    public class StockPickingCreate
    {
        public Guid? BackorderId { get; set; }
        public Guid LocationId { get; set; }
        public Guid LocationDestId { get; set; }
        public Guid PickingTypeId { get; set; }
        public Guid? PartnerId { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? DateDeadline { get; set; }
    }

    public class StockPickingReceipt
    {
        public Guid LocationDestId { get; set; }
        public Guid PickingTypeId { get; set; }
        public Guid? PartnerId { get; set; }
        public string? Note { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? DateDeadline { get; set; }
    }

    public class StockPickingUpdate
    {
        public Guid Id { get; set; }
        public Guid? LocationId { get; set; }
        public Guid? LocationDestId { get; set; }
        public Guid? PartnerId { get; set; }
        public string? Name { get; set; }
        public string? Note { get; set; }
        public DateTime? ScheduledDate { get; set; }
        public DateTime? DateDeadline { get; set; }
    }
}
