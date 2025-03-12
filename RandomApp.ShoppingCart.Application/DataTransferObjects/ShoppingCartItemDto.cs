namespace RandomApp.ShoppingCartManagement.Application.DataTransferObjects
{
    public record ShoppingCartItemDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }

        public decimal TotalPrice => Price * Quantity;
    }
}
