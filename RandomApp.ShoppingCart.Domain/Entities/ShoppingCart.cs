using RandomApp.ShoppingCartManagement.Domain.ValueObjects;

namespace RandomApp.ShoppingCartManagement.Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public DateTime? LastModified { get; private set; }
        
        private readonly List<ShoppingCartItem> _items = new List<ShoppingCartItem>();

        public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();
    }
}
