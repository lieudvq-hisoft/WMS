﻿using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class StockMoveModel
    {
        public Guid Id { get; set; }
        public ProductProductModel ProductProduct { get; set; }
        public UomUomModel UomUom { get; set; }
        public StockLocationModel Location { get; set; }
        public StockLocationModel LocationDest { get; set; }
        public Guid? PickingId { get; set; }
        public string Name { get; set; }
        public StockMoveState? State { get; set; }
        public string Reference { get; set; }
        public string? DescriptionPicking { get; set; }
        public decimal ProductQty { get; set; }
        public decimal? ProductUomQty { get; set; }
        public decimal? Quantity { get; set; }
        public DateTime? ReservationDate { get; set; }
    }

    public class StockMoveCreate
    {
        public Guid ProductId { get; set; }
        public Guid ProductUomId { get; set; }
        public Guid PickingId { get; set; }
        public Guid LocationId { get; set; }
        public Guid LocationDestId { get; set; }
        public string? DescriptionPicking { get; set; }
        public decimal ProductUomQty { get; set; }
    }
}

