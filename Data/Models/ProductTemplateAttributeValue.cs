using System;
using Data.Entities;
using Data.Enums;

namespace Data.Models
{
    public class ProductTemplateAttributeValueModel
    {
        public ProductAttributeValueModel ProductAttributeValue { get; set; }
    }

    public class ProductTemplateAttributeValueCreate
    {
        public Guid AttributeLineId { get; set; }
        public List<Guid> ProductAttributeValueIds { get; set; }
    }
}

