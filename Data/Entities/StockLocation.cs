using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class StockLocation : StockBaseEntity
    {
        public Guid? LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual StockLocation? ParentLocation { get; set; }

        public Guid? RemovalStrategyId { get; set; }
        [ForeignKey("RemovalStrategyId")]
        public virtual ProductRemoval? RemovalStrategy { get; set; }

        [Required]
        public string Name { get; set; }

        public string? CompleteName { get; set; }

        public string? ParentPath { get; set; }

        public string? Barcode { get; set; }

        [Required]
        public LocationType Usage { get; set; }

        public virtual ICollection<StockQuant> StockQuants { get; set; }

        public virtual ICollection<StockWarehouse> StockWarehouses { get; set; }
    }
}