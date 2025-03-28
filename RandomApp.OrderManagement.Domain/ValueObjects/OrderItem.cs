using Common.Shared.Exceptions;

namespace RandomApp.OrderManagement.Domain.ValueObjects
{
    public record OrderItem
    {
        public int ProductId { get; private init; }

        public string Name { get; private init; }
        public decimal UnitPrice { get; private init; }

        public int Quantity { get; private init; }

        public decimal LineTotal => UnitPrice * Quantity;

        private OrderItem() { }

        public OrderItem(int productId, string name, decimal unitPrice, int quantity)
        {
            if (quantity < 0)
                throw new DomainException("Quantity must be positive");
            if (unitPrice < 0)
                throw new DomainException("Price cannot be negative");
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty");

            ProductId = productId;
            Name = name.Trim();
            UnitPrice = unitPrice;
            Quantity = quantity;

        }
    }
}
