using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Data.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Entities
{
    public class UomUom
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Guid? CreateUid { get; set; }

        public Guid? WriteUid { get; set; }

        [Required]
        public string UomType {
            get => _uomType;
            set
            {
                _uomType = value;
                ComputeFactor();
            }
        }
        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "numeric")]
        public decimal Factor { get; set; }

        [Required]
        [Column(TypeName = "numeric")]
        public decimal Rounding { get; set; }

        public bool? Active { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CategoryId")]
        public virtual UomCategory Category { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        private string _uomType;
        private void ComputeFactor()
        {
            switch (UomType)
            {
                case "Reference":
                    Factor = 1;
                    break;
                case "Bigger":
                    Factor = Factor != 0 ? 1 / Factor : 0;
                    break;
                default:
                    Factor = Factor;
                    break;
            }
        }
    }
}