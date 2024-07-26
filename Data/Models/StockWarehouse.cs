using System;
using System.ComponentModel.DataAnnotations;
using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class StockWarehouseModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class StockWarehouseInfo : StockWarehouseModel
    {
        public StockLocationModel ViewLocation { get; set; }
        public StockLocationModel LotStock { get; set; }
        public StockLocationModel WhInputStockLoc { get; set; }
        public StockLocationModel WhQcStockLoc { get; set; }
        public StockLocationModel WhOutputStockLoc { get; set; }
        public StockLocationModel WhPackStockLoc { get; set; }
    }

    public class StockWarehouseCreate
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class StockWarehouseUpdate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
}

