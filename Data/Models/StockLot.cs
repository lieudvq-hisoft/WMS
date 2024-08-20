using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class StockLotModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
    }

    public class StockLotInfo : StockLotModel
    {
        public ProductProduct ProductProduct { get; set; }
        public StockLocation? StockLocation { get; set; }
    }

    public class StockLotCreate
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
    }

    public class StockLotUpdate
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string? Note { get; set; }
    }
}

