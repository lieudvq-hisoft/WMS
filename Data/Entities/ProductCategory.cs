using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Data.Entities
{
    public class ProductCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string? CompleteName { get; set; }

        public string? ParentPath { get; set; }

        public Guid? CreateUid { get; set; }

        public Guid? ParentId { get; set; }

        public Guid? RemovalStrategyId { get; set; }

        public Guid? WriteUid { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        [ForeignKey("RemovalStrategyId")]
        public virtual ProductRemoval ProductRemoval { get; set; }

        [ForeignKey("ParentId")]
        public virtual ProductCategory ParentCategory { get; set; }
    }
}