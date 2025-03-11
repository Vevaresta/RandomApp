using RandomApp.ProductManagement.Domain.ValueObjects;
using RandomApp.ProductManagement.Domain.Enums;
using Common.Shared.Exceptions;

namespace RandomApp.ProductManagement.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }

        public int OriginalApiId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public Price Price { get; private set; }

        public SKU SKU { get; private set; }

        public Category Category { get; private set; }
        public ProductDescription ProductDescription { get; private set; }

        public string Image { get; private set; }

        private Product()
        {

        }

        public static Product Create(

            int originalApiId,
            string name,
            Price price,
            SKU sku,
            Category category,
            ProductDescription description,
            string image)

        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name can't be empty");

            if (originalApiId <= 0)
                throw new DomainException("Original API ID must be positive");

            return new Product
            {
                Id = Guid.NewGuid(),
                OriginalApiId = originalApiId,
                Name = name,
                Price = price ?? throw new DomainException("Price can't be null"),
                SKU = sku ?? throw new DomainException("SKU can't be null"),
                Category = category,
                ProductDescription = description ?? throw new DomainException("Description can't be null"),
                Image = image ?? string.Empty
            };
        }

        public void UpdateProduct(string name, Price price, Category category, ProductDescription description, string image)
        {
            Name = name;
            Price = price;
            Category = category;
            ProductDescription = description;
            Image = image;
        }

        public void UpdatePrice(Price newPrice)
        {
            Price = newPrice ?? throw new DomainException("New price can't be null");
        }

        public void UpdateName(string newName)
        {
            if (string.IsNullOrWhiteSpace(newName))
                throw new DomainException("Product name can't be empty");

            Name = newName;
        }

        public void UpdateDescription(ProductDescription newDescription)
        {
            ProductDescription = newDescription ?? throw new DomainException("New description can't be null");

        }

        public void UpdateCategory(Category newCategory)
        {
            Category = newCategory;
        }

        public void UpdateImage(string newImage)
        {
            Image = newImage ?? string.Empty;
        }
    }

}

