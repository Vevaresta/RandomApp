using Common.Shared.Exceptions;

namespace RandomApp.ProductManagement.Domain.ValueObjects
{
    public record ProductDescription
    {
        private const int MaxLength = 100;
        public string Value { get; private init; }

        public ProductDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new DomainException("Description can't be empty.");
            }

            if (description.Length > MaxLength)
            {
                throw new DomainException($"Descriptions can't exceed {MaxLength} characters");
            }

            Value = description;
        }
    }
}
