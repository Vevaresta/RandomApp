using Common.Shared.Exceptions;

namespace RandomApp.OrderManagement.Domain.ValueObjects
{
    public record OrderItem
    {
        public int ProductId { get; private init; }

        public string Name { get; private init; }
        public decimal UnitPrice { get; private init; }

        public int Quantity { get; private init; }

        public decimal Discount { get; private init; }

        public decimal TotalPriceItem => (UnitPrice * Quantity) - Discount;

        private OrderItem() { }

        public OrderItem(int productId, string name, decimal unitPrice, int quantity, decimal discount)
        {
            if (quantity < 0)
                throw new DomainException("Quantity must be positive");
            if (unitPrice < 0)
                throw new DomainException("Price cannot be negative");
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Product name cannot be empty");
            if (discount < 0 || discount > unitPrice * quantity)
                throw new DomainException("Discount is invalid");

            ProductId = productId;
            Name = name.Trim();
            UnitPrice = unitPrice;
            Quantity = quantity;
            Discount = discount;
        }
    }
}
