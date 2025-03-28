using Common.Shared.Exceptions;

namespace RandomApp.OrderManagement.Domain.ValueObjects
{
    public record BillingAddress
    {
        public string Address { get; private init; }

        public string City { get; private init; }
        public int PostalCode { get; private init; }
        public string Country { get; private init; }

        public string CardholderName { get; private init; }

        public BillingAddress(string address, string city, int postalCode, string country, string cardholderName)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new DomainException("Address cannot be empty");

            if (string.IsNullOrWhiteSpace(city))
                throw new DomainException("City name is required");

            if (postalCode <= 0)
                throw new DomainException("Postal code must be positive");

            if (string.IsNullOrWhiteSpace(country))
                throw new DomainException("Country name is required");

            if (!string.IsNullOrWhiteSpace(cardholderName))
                throw new DomainException("CardholderName is required"); 

            Address = address;
            City = city;
            PostalCode = postalCode;
            Country = country;
            CardholderName = cardholderName;
        }
    }
}
