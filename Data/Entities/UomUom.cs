using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

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
                _onchangeUomType();
            }
        }
        [Required]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "numeric")]
        public decimal Factor { get; set; }

        [NotMapped]
        public decimal FactorInv => Factor != 0 ? 1 / Factor : 0;

        [NotMapped]
        public decimal Ratio
        {
            get => _computeRatio();
            set
            {
                _setRatio(value);
            }
        }

        [Required]
        [Column(TypeName = "numeric")]
        public decimal Rounding { get; set; } = (decimal)0.01;

        public bool? Active { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CategoryId")]
        public virtual UomCategory Category { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        public virtual ICollection<ProductTemplate> ProductTemplates { get; set; }
        public virtual ICollection<StockMove> StockMoves { get; set; }

        [NotMapped]
        private string _uomType;
        [NotMapped]
        private decimal _ratio;

        private void _onchangeUomType()
        {
            if (UomType == "Bigger")
            {
                Factor = FactorInv != 0 ? 1 / FactorInv : 0;
            }
            else
            {
                _ratio = Factor;
            }
        }
        private decimal _computeRatio()
        {
            if (UomType == "Reference")
                return 1;
            else if (UomType == "Bigger")
                return FactorInv;
            else
                return Factor;
        }
        private void _setRatio(decimal value)
        {
            if (value == 0)
                throw new ArgumentException("The value of ratio could not be Zero");

            _ratio = value;

            if (UomType == "Reference")
            {
                Factor = 1;
            }
            else if (UomType == "Bigger")
            {
                Factor = 1 / _ratio;
            }
            else
            {
                Factor = _ratio;
            }
        }
    }
}