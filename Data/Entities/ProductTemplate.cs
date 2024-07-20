using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class ProductTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid CategId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DetailedType { get; set; }

        [Required]
        public string Tracking { get; set; } = "None";

        public string Description { get; set; }

        public bool? Active { get; set; } = true;

        public Guid UomId { get; set; }

        public Guid? CreateUid { get; set; }

        public Guid? WriteUid { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        [ForeignKey("CategId")]
        public virtual ProductCategory ProductCategory { get; set; }

        [ForeignKey("UomId")]
        public virtual UomUom UomUom { get; set; }

        public virtual ICollection<ProductProduct> ProductProducts { get; set; }

        public virtual ICollection<ProductTemplateAttributeLine> ProductTemplateAttributeLines { get; set; }
    }
}