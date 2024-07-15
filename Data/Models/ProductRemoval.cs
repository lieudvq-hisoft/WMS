using System;
using Data.Enums;

namespace Data.Models
{
	public class ProductRemovalModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Method { get; set; }
    }

    public class ProductRemovalCreate
    {
        public string Name { get; set; }
        public RemovalMethod Method { get; set; }
    }

    public class ProductRemovalUpdate
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public RemovalMethod? Method { get; set; }
    }
}

