using System;
using Data.Entities;
using Data.Enums;

namespace Data.Models
{
	public class ProductCategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? CompleteName { get; set; }
        public string? ParentPath { get; set; }
    }

    public class ProductCategoryInfo : ProductCategoryModel
    {
        public ProductCategoryModel? ParentCategory { get; set; }

    }

    public class ProductCategoryCreate
    {
        public string Name { get; set; }
        public Guid RemovalStrategyId { get; set; }
        public Guid? ParentId { get; set; }
    }

    public class ProductCategoryUpdate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

    public class ProductCategoryParentUpdate
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
    }
}

