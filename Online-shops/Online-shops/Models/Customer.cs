using System.ComponentModel.DataAnnotations;

namespace Online_shops.Models
{
    public class Customer
    {

        [Key]
        public Guid CustomerId { get; set; }
        public string customerName { get; set; }
        public int mobile { get; set; }
        public string emailID { get; set; }
    }
}
