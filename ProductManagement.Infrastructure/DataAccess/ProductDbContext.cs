using Microsoft.EntityFrameworkCore;
using Random.App.ProductManagement.Domain.Entities;


namespace Random.App.ProductManagement.Infrastructure.DataAccess
{
    public class ProductDbContext : DbContext
    {

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        
        }
    }
}
