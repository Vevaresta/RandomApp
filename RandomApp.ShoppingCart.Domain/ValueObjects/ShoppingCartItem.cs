using RandomApp.ShoppingCartManagement.Domain.Entities;

namespace RandomApp.ShoppingCartManagement.Domain.ValueObjects
{
    public class ShoppingCartItem
    {
        public int Id { get; set; }
        public int ShoppingCartId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
    }
}
