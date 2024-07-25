using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class StockPickingType : StockBaseEntity
    {

        [Required]
        public Guid WarehouseId { get; set; }
        [ForeignKey("WarehouseId")]
        public virtual StockWarehouse Warehouse { get; set; }

        public Guid? ReturnPickingTypeTd { get; set; }
        [ForeignKey("ReturnPickingTypeTd")]
        public virtual StockPickingType? ReturnPickingType { get; set; }

        [Required]
        public StockPickingTypeCode Code { get; set; }

        public string? Barcode { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public CreateBackorderType CreateBackorder { get; set; }
    }
}