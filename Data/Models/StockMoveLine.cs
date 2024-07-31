using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class StockMoveLineView
    {
        public string Reference { get; set; }
        public string ProductProduct { get; set; }
        public string UomUom { get; set; }
        public StockMoveState? State { get; set; }
        public decimal? QuantityProductUom { get; set; }
        public decimal? Quantity { get; set; }
        public string Location { get; set; }
        public string LocationDest { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}

