using System.ComponentModel.DataAnnotations;

namespace Online_shops.Models
{
    public class Product
    {
        [Key]
        public Guid productID { get; set; }
        public string productName { get; set; }
        public int quantity { get; set; }

        public bool isActive { get; set; }
    }
}
