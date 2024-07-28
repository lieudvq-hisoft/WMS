using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class StockMoveLineView
    {
        public ProductProductModel ProductProduct { get; set; }
        public string Reference { get; set; }
        public string UomUom { get; set; }
        public StockMoveState? State { get; set; }
        public int? QuantityProductUom { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public string LocationDest { get; set; }
    }
}

