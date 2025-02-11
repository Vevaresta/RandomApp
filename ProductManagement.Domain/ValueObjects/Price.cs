using RandomApp.ProductManagement.Domain.Exceptions;

namespace RandomApp.ProductManagement.Domain.ValueObjects
{
    public record Price
    {
        public string Currency { get; private init; }
        public decimal Amount { get; private init; }

        public Price(decimal amount, string currency)
        {
            if (amount <= 0)
                throw new DomainException("Price cannot be negative");

            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Currency is required");
            
            Amount = amount;
            Currency = currency;
        }
    }
}
