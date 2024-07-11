using System;
using Data.Model;

namespace Data.Models
{
	public class UomCategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<UomUomCollection> UomUoms { get; set; }
    }

    public class UomCategoryCreate
    {
        public string Name { get; set; }
    }

    public class UomCategoryUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UomUomCollection
    {
        public Guid Id { get; set; }
        public string UomType { get; set; }
        public string Name { get; set; }
        public decimal Rounding { get; set; }
        public bool? Active { get; set; }
        public decimal Ratio { get; set; }

    }
}

