using System;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Data.Enums;

namespace Data.Entities
{
    public class UomCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid? CreateUid { get; set; }

        public Guid? WriteUid { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? CreateDate { get; set; }

        public DateTime? WriteDate { get; set; }

        [ForeignKey("CreateUid")]
        public virtual User CreateUser { get; set; }

        [ForeignKey("WriteUid")]
        public virtual User WriteUser { get; set; }

        public virtual ICollection<UomUom> UomUoms { get; set; }

        public void UpdateReferenceUom(Guid referenceUomId)
        {
            if (UomUoms.Count == 1)
            {
                var uom = UomUoms.First();
                uom.UomType = "Reference";
                uom.Factor = 1;
            }
            else
            {
                var referenceCount = UomUoms.Count(uom => uom.UomType == "Reference");
                if (referenceCount == 0 && Id != Guid.Empty)
                {
                    throw new Exception($"UoM category {Name} must have at least one reference unit of measure.");
                }

                var newReference = UomUoms.FirstOrDefault(uom => uom.UomType == "Reference" && uom.Id == referenceUomId);

                if (newReference != null)
                {
                    foreach (var uom in UomUoms.Where(u => u.Id != newReference.Id))
                    {
                        bool hasStockMove = uom.StockMoves.Any();
                        if (hasStockMove)
                        {
                            throw new Exception("You cannot change the ratio of this unit of measure as some products with this UoM have already been moved or are currently reserved.");
                        }
                        uom.Factor = uom.Factor / (newReference.Factor != 0 ? newReference.Factor : 1);
                        uom.UomType = uom.Factor > 1 ? "Smaller" : "Bigger";
                    }
                    newReference.Factor = 1;
                }
            }
        }
    }
}

