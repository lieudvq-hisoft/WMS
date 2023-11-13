using Data.Model;

namespace Data.Models
{
	public class InventoryModel
	{
        public Guid Id { get; set; }
        public virtual LocationModel Location { get; set; }
        public string Note { get; set; }
        public int QuantityOnHand { get; set; } = 0;
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
    }
}

