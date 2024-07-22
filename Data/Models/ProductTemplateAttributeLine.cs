using System;
using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class ProductTemplateAttributeLineModel
    {
        public Guid Id { get; set; }
        public Guid AttributeId { get; set; }
    }

    public class ProductTemplateAttributeLineInfo : ProductTemplateAttributeLineModel
    {
        public ProductAttributeModel ProductAttribute { get; set; }
        public List<ProductTemplateAttributeValueModel> ProductTemplateAttributeValues { get; set; }
    }

    public class ProductTemplateAttributeLineCreate
    {
        public Guid AttributeId { get; set; }
        public Guid ProductTmplId { get; set; }
    }

    public class ProductTemplateAttributeLineUpdate
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }
    }

    public class ProductTemplateAttributeValueModel
    {
        public ProductAttributeValueModel ProductAttributeValue { get; set; }
    }
}

