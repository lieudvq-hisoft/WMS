using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class ProductRemoval
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Method { get; set; }

        public Guid? CreateUid { get; set; }

        public Guid? WriteUid { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        public virtual ICollection<ProductCategory> ProductCategories { get; set; }

    }
}