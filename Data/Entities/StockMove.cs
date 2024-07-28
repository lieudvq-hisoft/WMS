using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class StockMove : StockBaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductProduct ProductProduct { get; set; }

        [Required]
        public Guid ProductUomId { get; set; }
        [ForeignKey("ProductUomId")]
        public virtual UomUom ProductUom { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual StockLocation Location { get; set; }

        [Required]
        public Guid LocationDestId { get; set; }
        [ForeignKey("LocationDestId")]
        public virtual StockLocation LocationDest { get; set; }

        public Guid? PickingId { get; set; }
        [ForeignKey("PickingId")]
        public virtual StockPicking? StockPicking { get; set; }

        [Required]
        public string Name { get; set; }

        public StockMoveState? State { get; set; }

        public string? Reference { get; set; }

        public string? DescriptionPicking { get; set; }

        [Required]
        public int ProductQty { get; set; }

        [Required]
        public int ProductUomQty { get; set; }

        public int? Quantity { get; set; }

        public DateTime? ReservationDate { get; set; }

    }
}