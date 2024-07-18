using System;
using Data.Enums;

namespace Data.Models
{
	public class ProductAttributeModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductAttributeCreate
    {
        public string Name { get; set; }
    }

    public class ProductAttributeUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

