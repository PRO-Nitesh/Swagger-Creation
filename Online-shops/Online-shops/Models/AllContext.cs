using Microsoft.EntityFrameworkCore;

namespace Online_shops.Models
{
    public class AllContext :DbContext
    {
        public AllContext(DbContextOptions<AllContext> options) : base(options) {
        
        }
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Order> Orders { get; set; }




    }
}
