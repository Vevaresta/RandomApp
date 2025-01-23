namespace RandomApp.ShoppingCart.Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<ShoppingCartItem> Items { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
