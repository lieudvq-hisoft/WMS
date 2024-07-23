
using Data.Entities;

namespace Data.Models
{

    public class ProductProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public  string VariantCombination { get; set; }

    }
    public class ProductProductCreate
    {
        public Guid ProductTmplId { get; set; }
    }
}

