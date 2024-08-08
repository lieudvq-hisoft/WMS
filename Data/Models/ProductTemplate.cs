
using Microsoft.AspNetCore.Http;

namespace Data.Models
{
	public class ProductTemplateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DetailedType { get; set; }
        public string Tracking { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class ProductTemplateInfo : ProductTemplateModel
    {
        public ProductCategoryModel ProductCategory { get; set; }
        public UomUomModel UomUom { get; set; }
        public int? TotalVariant { get; set; }
        public decimal? QtyAvailable { get; set; }

    }

    public class ProductTemplateCreate
    {
        public Guid CategId { get; set; }
        public Guid UomId { get; set; }
        public string Name { get; set; }
        public string DetailedType { get; set; }
        public string Tracking { get; set; }
        public string Description { get; set; }
    }

    public class ProductTemplateUpdate
    {
        public Guid Id { get; set; }
        public Guid? CategId { get; set; }
        public Guid? UomId { get; set; }
        public string? Name { get; set; }
        public string? DetailedType { get; set; }
        public string? Tracking { get; set; }
        public string? Description { get; set; }
    }

    public class SuggestProductVariant
    {
        public Guid Id { get; set; }
        public Guid? CategId { get; set; }
        public Guid? UomId { get; set; }
        public string? Name { get; set; }
        public string? DetailedType { get; set; }
        public string? Tracking { get; set; }
        public string? Description { get; set; }
    }

    public class ProductVariantCreate
    {
        public Guid ProductTemplateId { get; set; }
        public List<Guid> PtavIds { get; set; }
    }

    public class ProductTemplateImageUpdate
    {
        public IFormFile File { get; set; }
    }
}

