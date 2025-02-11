using RandomApp.ProductManagement.Domain.Exceptions;

namespace RandomApp.ProductManagement.Domain.ValueObjects
{
    public record SKU
    {
        private const int DefaultLength = 15;
        private SKU(string value) => Value = value;
        public string Value { get; private init; }

        // can be only created through the Create method -> forces factory method pattern
        public static SKU? Create(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new DomainException("Value cannot be null");

            if (value.Length > DefaultLength)
                throw new DomainException("Length has to be less than 15 characters");

            return new SKU(value);
        }
    }
}
