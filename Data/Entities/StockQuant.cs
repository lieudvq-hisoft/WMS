using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class StockQuant : StockBaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductProduct ProductProduct { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual StockLocation StockLocation { get; set; }

        public Guid? LotId { get; set; }
        [ForeignKey("LotId")]
        public virtual StockLot? StockLot { get; set; }

        public DateTime? InventoryDate { get; set; }

        public int Quantity { get; set; } = 0;

        public int? InventoryQuantity { get; set; }

        public int? InventoryDiffQuantity { get; set; }

        public bool? InventoryQuantitySet { get; set; } = false;

        public virtual ICollection<StockMoveLine> StockMoveLines { get; set; }
    }
}