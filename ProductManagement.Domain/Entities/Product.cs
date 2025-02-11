using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace RandomApp.ProductManagement.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public int OriginalApiId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public string Category { get; set; }
        public string Description { get; set; }

        public string Image { get; set; }
    }
}
