using Microsoft.EntityFrameworkCore;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.ValueObjects;
using RandomApp.ShoppingCartManagement.Infrastructure.Configuration;


namespace RandomApp.ShoppingCartManagement.Infrastructure.Persistance
{
    public class ShoppingCartDbContext : DbContext
    {
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public ShoppingCartDbContext(DbContextOptions<ShoppingCartDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.ApplyConfiguration(new ShoppingCartValueObjectMapping());

            modelBuilder.Entity<ShoppingCart>(entity =>
            {
                entity.ToTable("ShoppingCarts").HasKey(cart => cart.Id);

                entity.Property(cart => cart.UserId).IsRequired();
                entity.Property(cart => cart.CreatedAt).IsRequired();
                entity.Property(cart => cart.LastModified);

                entity.OwnsMany(cart => cart.Items, itemBuilder =>
                {
                    itemBuilder.WithOwner().HasForeignKey("ShoppingCartId");
                    itemBuilder.Property<int>("Id").ValueGeneratedOnAdd();
                    itemBuilder.HasKey("Id");

                    itemBuilder.Property(item => item.ProductId).IsRequired();
                    itemBuilder.Property(item => item.Name).IsRequired().HasMaxLength(256);
                    itemBuilder.Property(item => item.Price).IsRequired().HasColumnType("decimal(18,2)");
                    itemBuilder.Property(item => item.Image).IsRequired(false);
                    itemBuilder.Property(item => item.Quantity).IsRequired();
                    itemBuilder.Property(item => item.IsAvailable);

                    itemBuilder.HasIndex("ShoppingCartId", nameof(ShoppingCartItem.ProductId));
                });
            });
        }
    }
}
