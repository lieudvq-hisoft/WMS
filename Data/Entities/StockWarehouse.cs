using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class StockWarehouse : StockBaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Code { get; set; }

        [Required]
        public Guid ViewLocationId { get; set; }
        [ForeignKey("ViewLocationId")]
        public virtual StockLocation ViewLocation { get; set; }

        [Required]
        public Guid LotStockId { get; set; }
        [ForeignKey("LotStockId")]
        public virtual StockLocation LotStock { get; set; }

        public Guid? WhInputStockLocId { get; set; }
        [ForeignKey("WhInputStockLocId")]
        public virtual StockLocation? WhInputStockLoc { get; set; }

        public Guid? WhQcStockLocId { get; set; }
        [ForeignKey("WhQcStockLocId")]
        public virtual StockLocation? WhQcStockLoc { get; set; }

        public Guid? WhOutputStockLocId { get; set; }
        [ForeignKey("WhOutputStockLocId")]
        public virtual StockLocation? WhOutputStockLoc { get; set; }

        public Guid? WhPackStockLocId { get; set; }
        [ForeignKey("WhPackStockLocId")]
        public virtual StockLocation? WhPackStockLoc { get; set; }

        public virtual ICollection<StockPickingType> StockPickingTypes { get; set; }

    }
}