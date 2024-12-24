namespace RandomApp.ProductManagement.Domain.Models
{
    public class SyncResult
    {
        public string Message { get; set; } = string.Empty;

        public int NewProductsAdded { get; set; }
        public int ProductsUpdated { get; set; }
        public bool Success {  get; set; }
    }
}
