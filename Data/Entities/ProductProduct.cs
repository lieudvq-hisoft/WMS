using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class ProductProduct
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ProductTmplId { get; set; }

        public bool? Active { get; set; } = true;

        public Guid? CreateUid { get; set; }

        public Guid? WriteUid { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        [ForeignKey("ProductTmplId")]
        public virtual ProductTemplate ProductTemplate { get; set; }

        public virtual ICollection<ProductVariantCombination> ProductVariantCombinations { get; set; }
        public virtual ICollection<StockQuant> StockQuants { get; set; }
        public virtual ICollection<StockMove> StockMoves { get; set; }

    }
}