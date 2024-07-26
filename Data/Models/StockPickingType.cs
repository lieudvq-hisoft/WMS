using System.ComponentModel.DataAnnotations;
using Data.Enums;

namespace Data.Models
{
	public class StockPickingTypeModel
    {
        public Guid Id { get; set; }
        public StockPickingTypeCode Code { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
    }
}

