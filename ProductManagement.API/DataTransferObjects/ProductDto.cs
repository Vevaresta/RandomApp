namespace RandomApp.ProductManagement.Application.DataTransferObjects
{
    public class ProductDto
    {
        public int Id { get; set; }

        public int OriginalApiId { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }

        public string Currency {  get; set; }
        public string SKU { get; set; }

        public string Category { get; set; }
        public string ProductDescription { get; set; }

        public string Image { get; set; }

    }
}
