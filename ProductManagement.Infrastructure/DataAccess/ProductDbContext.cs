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
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Name = "Coca Cola Zero", Description = "Sugar Free Drink" },
                new Product { Id = 2, Name = "Coca Cola Light", Description = "Sugar Free Drink" },
                new Product { Id = 3, Name = "Snickers", Description = "Chocholate Bar" },
                new Product { Id = 4, Name = "Mars", Description = "Chocholate Bar" },
                new Product { Id = 5, Name = "Kinder Bueno", Description = "Chocholate Bar" }
        );
        }
    }
}
