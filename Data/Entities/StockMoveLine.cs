using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class StockMoveLine : StockBaseEntity
    {
        [Required]
        public Guid MoveId { get; set; }
        [ForeignKey("MoveId")]
        public virtual StockMove StockMove { get; set; }

        [Required]
        public Guid ProductUomId { get; set; }
        [ForeignKey("ProductUomId")]
        public virtual UomUom ProductUom { get; set; }

        [Required]
        public Guid QuantId { get; set; }
        [ForeignKey("QuantId")]
        public virtual StockQuant StockQuant { get; set; }

        public StockMoveState? State { get; set; }

        public decimal? QuantityProductUom { get; set; }

        public decimal Quantity { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual StockLocation Location { get; set; }

        [Required]
        public Guid LocationDestId { get; set; }
        [ForeignKey("LocationDestId")]
        public virtual StockLocation LocationDest { get; set; }

    }
}