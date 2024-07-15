using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class ProductTemplateAttributeValue
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid AttributeLineId { get; set; }

        public Guid ProductAttributeValueId { get; set; }

        public bool? Active { get; set; } = true;

        public Guid? CreateUid { get; set; }

        public Guid? WriteUid { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        [ForeignKey("AttributeLineId")]
        public virtual ProductTemplateAttributeLine ProductTemplateAttributeLine { get; set; }

        [ForeignKey("ProductAttributeValueId")]
        public virtual ProductAttributeValue ProductAttributeValue { get; set; }

        public virtual ICollection<ProductVariantCombination> ProductVariantCombinations { get; set; }

    }
}