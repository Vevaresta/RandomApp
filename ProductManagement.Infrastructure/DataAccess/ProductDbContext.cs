using Microsoft.EntityFrameworkCore;
using RandomApp.ProductManagement.Domain.Entities;


namespace RandomApp.ProductManagement.Infrastructure.DataAccess
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
