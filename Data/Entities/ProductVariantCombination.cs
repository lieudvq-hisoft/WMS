using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class ProductVariantCombination
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid ProductProductId { get; set; }

        public Guid ProductTemplateAttributeValueId { get; set; }

        public bool? Active { get; set; } = true;

        public Guid? CreateUid { get; set; }

        public Guid? WriteUid { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        [ForeignKey("ProductProductId")]
        public virtual ProductProduct ProductProduct { get; set; }

        [ForeignKey("ProductTemplateAttributeValueId")]
        public virtual ProductTemplateAttributeValue ProductTemplateAttributeValue { get; set; }
    }
}