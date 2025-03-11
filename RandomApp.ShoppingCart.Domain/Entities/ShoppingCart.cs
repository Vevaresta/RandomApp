using Common.Shared.Exceptions;
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

        private ShoppingCart() { }

        public static ShoppingCart Create(int userId)
        {
            if (userId <= 0)
                throw new DomainException("User ID must be a positive number");

            return new ShoppingCart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
        }

        public void AddItem(int productId, int quantity, decimal unitPrice)
        {
            var existingItem = _items.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                _items.Remove(existingItem);
                _items.Add(new ShoppingCartItem(
                    productId,
                    existingItem.Quantity + quantity, unitPrice));
            }
            else
            {
                _items.Add(new ShoppingCartItem(productId, quantity, unitPrice));
            }

            LastModified = DateTime.UtcNow;
        }

        public void RemoveItem(int productId)
        {
            var item = _items.FirstOrDefault(item => item.ProductId == productId);
            if (item != null)
            {
                _items.Remove(item);
                LastModified = DateTime.UtcNow;
            }
        }

        public void UpdateItemQuantity(int productId, int quantity)
        {
            var existingItem = _items.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                _items.Remove(existingItem);
                _items.Add(new ShoppingCartItem(productId, quantity, existingItem.Price));
                 LastModified = DateTime.UtcNow;
            }
        }
    }
}
