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

    public class StockWarehouseInfo : StockWarehouse
    {

    }

    public class StockWarehouseCreate
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class StockWarehouseUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }
}

