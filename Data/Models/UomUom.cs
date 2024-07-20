using System;
using Data.Enums;

namespace Data.Models
{
	public class UomUomModel
    {
        public Guid Id { get; set; }
        public string UomType { get; set; }
        public string Name { get; set; }
        public decimal Rounding { get; set; }
        public bool? Active { get; set; }
        public decimal Ratio { get; set; }
    }

    public class UomUomCreate
    {
        public Guid CategoryId { get; set; }
        public UomType UomType { get; set; }
        public string Name { get; set; }
        public decimal Factor { get; set; }
        public decimal Rounding { get; set; }
        public bool? Active { get; set; }
    }

    public class UomUomUpdate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal? Rounding { get; set; }
        public bool? Active { get; set; }
    }

    public class UomUomUpdateType
    {
        public Guid Id { get; set; }
        public UomType UomType { get; set; }
    }

    public class UomUomUpdateFactor
    {
        public Guid Id { get; set; }
        public decimal Factor { get; set; }
    }
}

