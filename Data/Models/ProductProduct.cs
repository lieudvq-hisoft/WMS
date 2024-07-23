
namespace Data.Models
{

    public class ProductProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Pvc> Pvcs { get; set; }

    }
    public class ProductProductCreate
    {
        public Guid ProductTmplId { get; set; }
    }

    public class Pvc
    {
        public string Attribute { get; set; }
        public string Value { get; set; }
    }
}

