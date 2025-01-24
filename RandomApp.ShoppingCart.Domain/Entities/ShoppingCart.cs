namespace RandomApp.ShoppingCartManagement.Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        // virtual nav properties enable lazy loading of related entities
        public virtual ICollection<ShoppingCartItem> Items { get; set; }

        public ShoppingCart()
        {
            Items = new List<ShoppingCartItem>();
        }
    }
}
