using Common.Shared.Exceptions;
using RandomApp.ShoppingCartManagement.Domain.ValueObjects;

namespace RandomApp.ShoppingCartManagement.Domain.Entities
{
    public class ShoppingCart
    {
        public Guid Id { get; private set; }
        public int UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public DateTime? LastModified { get; private set; }

        private readonly List<ShoppingCartItem> _items = new List<ShoppingCartItem>();

        public IReadOnlyCollection<ShoppingCartItem> Items => _items.AsReadOnly();

        public decimal TotalPriceCart => _items.Sum(item => item.TotalPriceItem);

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

        public void AddItem(int productId, string name, int quantity, decimal price, string image)
        {
            var existingItem = _items.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                _items.Remove(existingItem);
                _items.Add(new ShoppingCartItem(
                    productId,
                    name,
                    price,
                    image,
                    existingItem.Quantity + quantity,
                    true));
            }
            else
            {
                _items.Add(new ShoppingCartItem(
                    productId,
                    name, 
                    price,
                    image, 
                    quantity, 
                    true));
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

        public void UpdateItemQuantity(int productId, string name, int quantity, decimal price, string image)
        {
            var existingItem = _items.FirstOrDefault(item => item.ProductId == productId);
            if (existingItem != null)
            {
                _items.Remove(existingItem);
                _items.Add(new ShoppingCartItem(
                    productId,
                    name,
                    price,
                    image,
                    quantity,
                    true));
                 LastModified = DateTime.UtcNow;
            }
        }
    }
}
