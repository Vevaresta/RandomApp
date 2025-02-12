using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using RandomApp.ProductManagement.Domain.ValueObjects;


namespace RandomApp.ProductManagement.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }

        public int OriginalApiId { get; private set; }

        public string Name { get; private set; } = string.Empty;

        public Price Price { get; private set; }

        public SKU SKU { get; private set; }

        public string Category { get; private set; }
        public ProductDescription ProductDescription{ get; private set; }

        public string Image { get; private set; }
    }
}
