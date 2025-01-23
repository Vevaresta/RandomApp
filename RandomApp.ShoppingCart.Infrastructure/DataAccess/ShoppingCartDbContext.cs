using Microsoft.EntityFrameworkCore;
using RandomApp.ShoppingCart.Domain.Entities;

namespace RandomApp.ShoppingCart.Infrastructure.DataAccess
{
    public class ShoppingCartDbContext : DbContext
    {
        public DbSet<Domain.Entities.ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
