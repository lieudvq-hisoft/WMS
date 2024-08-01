using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class StockPicking : StockBaseEntity
    {
        public Guid? BackorderId { get; set; }
        [ForeignKey("BackorderId")]
        public virtual StockPicking? Backorder { get; set; }

        [Required]
        public Guid LocationId { get; set; }
        [ForeignKey("LocationId")]
        public virtual StockLocation Location { get; set; }

        [Required]
        public Guid LocationDestId { get; set; }
        [ForeignKey("LocationDestId")]
        public virtual StockLocation LocationDest { get; set; }

        [Required]
        public Guid PickingTypeId { get; set; }
        [ForeignKey("PickingTypeId")]
        public virtual StockPickingType PickingType { get; set; }

        public Guid? PartnerId { get; set; }
        [ForeignKey("PartnerId")]
        public virtual ResPartner? Partner { get; set; }

        [Required]
        public string Name { get; set; }

        public PickingState? State { get; set; } = PickingState.Draft;

        public string? Note { get; set; }

        public DateTime? ScheduledDate { get; set; }

        public DateTime? DateDeadline { get; set; }

        public DateTime? DateDone { get; set; }

    }
}