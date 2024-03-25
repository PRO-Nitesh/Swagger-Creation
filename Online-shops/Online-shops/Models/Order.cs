using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Online_shops.Models
{
    public class Order
    {

        [Key]
        public Guid orderId { get; set; }
        [Required]
        [ForeignKey("Customers")]
        public string CustomerId { get; set; }
        [Required]
        [ForeignKey("Products")]
        public string productID { get; set; }
        public int quantity { get; set; }

        public Boolean IsCancel { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }    
    }
}
