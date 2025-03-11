using Common.Shared.Exceptions;

namespace RandomApp.ShoppingCartManagement.Domain.ValueObjects
{
    public class ShoppingCartItem
    {
        public int ProductId { get; private init; }
        public string Name { get; private init; } = string.Empty;
        public decimal Price { get; private init; }
        public string Image { get; private init; }
        public int Quantity { get; private init; }
        public bool IsAvailable { get; private init; }

        public decimal TotalPriceItem => Price * Quantity;

        private ShoppingCartItem() { }

        public ShoppingCartItem(int productId, string name, decimal price, string image, int quantity, bool isAvailable)
        {

            if (quantity <= 0)
                throw new DomainException("Quantity must be positive");

            if (price < 0)
                throw new ArgumentException("Price cannot be negative");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name cannot be empty");

            ProductId = productId;
            Name = name;
            Price = price;
            Image = image;
            Quantity = quantity;
            IsAvailable = isAvailable;
        }

    }
}
