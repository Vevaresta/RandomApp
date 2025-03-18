using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Domain.ValueObjects;

namespace RandomApp.ProductManagement.Infrastructure.Configuration
{
    public class ProductValueObjectMapping : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products").HasKey(product => product.Id);

            builder.OwnsOne(product => product.Price, price =>
            {
                price.Property(product => product.Amount).HasColumnName("Price");
                price.Property(product => product.Currency).HasColumnName("Currency");
            });

            builder.Property(product => product.SKU).HasConversion(
                sku => sku.Value,
                value => SKU.Create(value));

            builder.Property(product => product.ProductDescription).HasConversion(
                description => description.Value,
                value => new ProductDescription(value));

        }
    }
}
