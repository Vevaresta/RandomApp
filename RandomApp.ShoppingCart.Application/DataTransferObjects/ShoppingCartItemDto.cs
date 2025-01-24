namespace RandomApp.ShoppingCartManagement.Application.DataTransferObjects
{
    public class ShoppingCartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
