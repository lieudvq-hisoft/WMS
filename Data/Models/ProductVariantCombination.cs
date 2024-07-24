
namespace Data.Models
{
	public class ProductVariantCombinationModel
    {
        public Guid ProductTemplateAttributeValueId { get; set; }
    }

    public class ProductVariantCombinationCreate
    {
        public Guid ProductProductId { get; set; }
        public Guid ProductTemplateAttributeValueId { get; set; }
    }

    public class ProductVariantCombinationSuggest : ProductVariantCombinationModel
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
    }

}

