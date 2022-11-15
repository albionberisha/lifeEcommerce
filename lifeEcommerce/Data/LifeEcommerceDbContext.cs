using lifeEcommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace lifeEcommerce.Data
{
    public class LifeEcommerceDbContext : DbContext
    {
        public LifeEcommerceDbContext(DbContextOptions<LifeEcommerceDbContext> options) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
