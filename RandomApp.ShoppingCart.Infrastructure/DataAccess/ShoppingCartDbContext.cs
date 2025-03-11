using Microsoft.EntityFrameworkCore;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.ValueObjects;


namespace RandomApp.ShoppingCartManagement.Infrastructure.DataAccess
{
    public class ShoppingCartDbContext : DbContext
    {
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserId).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();

                entity.HasMany(e => e.Items)
                      .WithOne(i => i.ShoppingCart)
                      .HasForeignKey(i => i.ShoppingCartId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ShoppingCartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ShoppingCartId).IsRequired();
                entity.Property(e => e.ProductId).IsRequired();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
                entity.Property(e => e.Price).IsRequired();
                entity.Property(e => e.Quantity).IsRequired();
                entity.Property(e => e.Image).IsRequired(false);
            });

        }
    }
}
