using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class StockLot : StockBaseEntity
    {
        [Required]
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual ProductProduct ProductProduct { get; set; }

        [Required]
        public string Name { get; set; }

        public string? Note { get; set; }
    }
}