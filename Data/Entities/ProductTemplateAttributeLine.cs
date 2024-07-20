using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class ProductTemplateAttributeLine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid AttributeId { get; set; }

        public Guid ProductTmplId { get; set; }

        public bool? Active { get; set; } = true;

        public Guid? CreateUid { get; set; }

        public Guid? WriteUid { get; set; }

        public DateTime? CreateDate { get; set; } = DateTime.Now;

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        [ForeignKey("AttributeId")]
        public virtual ProductAttribute ProductAttribute { get; set; }

        [ForeignKey("ProductTmplId")]
        public virtual ProductTemplate ProductTemplate { get; set; }

        public virtual ICollection<ProductTemplateAttributeValue> ProductTemplateAttributeValues { get; set; }

    }
}