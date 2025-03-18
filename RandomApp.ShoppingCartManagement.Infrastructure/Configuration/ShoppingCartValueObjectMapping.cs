using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.ValueObjects;

namespace RandomApp.ShoppingCartManagement.Infrastructure.Configuration
{
    public class ShoppingCartValueObjectMapping : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
        {
            builder.ToTable("ShoppingCarts").HasKey(cart => cart.Id);

            builder.Property(cart => cart.UserId);
            builder.Property(cart => cart.CreatedAt);
            builder.Property(cart => cart.LastModified);

            builder.OwnsMany(cart => cart.Items, itemBuilder =>
            {
                itemBuilder.WithOwner().HasForeignKey("ShoppingCartId");
                itemBuilder.Property<int>("Id").ValueGeneratedOnAdd();
                itemBuilder.HasKey("Id");

                itemBuilder.Property(item => item.ProductId);
                itemBuilder.Property(item => item.Name);
                itemBuilder.Property(item => item.Price).HasColumnType("decimal(18,2)");
                itemBuilder.Property(item => item.Image);
                itemBuilder.Property(item => item.Quantity);
                itemBuilder.Property(item => item.IsAvailable);

                itemBuilder.HasIndex("ShoppingCartId", nameof(ShoppingCartItem.ProductId));
            });

        }
    }
}
