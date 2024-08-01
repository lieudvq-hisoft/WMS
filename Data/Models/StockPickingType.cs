using System.ComponentModel.DataAnnotations;
using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class StockPickingTypeModel
    {
        public Guid Id { get; set; }
        public StockPickingTypeCode Code { get; set; }
        public string Barcode { get; set; }
        public string Name { get; set; }
        public StockWarehouseModel Warehouse { get; set; }
    }

    public class StockPickingTypeInfo : StockPickingTypeModel
    {
        public StockWarehouseModel Warehouse { get; set; }
        public int TotalPickingReady { get; set; }
    }
}

