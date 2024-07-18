using System;
using Data.Enums;

namespace Data.Models
{
	public class ProductAttributeValueModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class ProductAttributeValueCreate
    {
        public string Name { get; set; }
        public Guid AttributeId { get; set; }
    }

    public class ProductAttributeValueUpdate
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}

