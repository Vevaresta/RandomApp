using Microsoft.EntityFrameworkCore;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Infrastructure.Configuration;


namespace RandomApp.ProductManagement.Infrastructure.Persistence
{
    public class ProductDbContext : DbContext
    {

        public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ProductValueObjectMapping());

        }
    }
}
