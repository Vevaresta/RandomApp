using Common.Shared.Repositories;
using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;
using NLog;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using AutoMapper;

namespace RandomApp.ShoppingCartManagement.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public async Task AddToCartAsync(ShoppingCartItemDto itemDto, int userId)
        {
            if (itemDto.Quantity <= 0)
            {
                _logger.Warn("Invalid quantity {quantity} for product {productId}", itemDto.Quantity);
                throw new ArgumentException("Quantity must be greater than 0");
            }

            _logger.Info("Fetching cart with user id {id}", userId);
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new ShoppingCart()
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Items = new List<ShoppingCartItem>()
                };

                _logger.Info("New shopping cart created for user {userId}", userId);
                await _shoppingCartRepository.AddAsync(cart);
                _logger.Info("Shopping cart with id {id} added to the table", cart.Id);
            }

            var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == itemDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += itemDto.Quantity;
                _logger.Info("Item {productId} quantity updated from {oldQuantity} to {newQuantity}",
                    existingItem.ProductId,
                    existingItem.Quantity - itemDto.Quantity,
                    existingItem.Quantity);
            }
            else
            {

                cart.Items.Add(new ShoppingCartItem
                {
                    ProductId = itemDto.ProductId,
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    Image = itemDto.Image,
                    Quantity = itemDto.Quantity,
                });

            }

            _logger.Info("Added item {productId} with quantity {quantity} to cart {cartId}",
                itemDto.ProductId,
                itemDto.Quantity,
                cart.Id);

            await _unitOfWork.CompleteAsync();
            _logger.Info("Saved cart {cartId} with {itemCount} items for user {userId}",
                cart.Id,
                cart.Items.Count,
                userId);
        }


        public async Task ClearCartAsync(int userId)
        {
            _logger.Info("Fetching cart with user id {id}", userId);
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart != null)
            {
                cart.Items.Clear();
                await _unitOfWork.CompleteAsync();
                _logger.Info("Cleared cart for user {userI}", userId);
            }

            else
            {
                _logger.Info("No cart found to clear for user {user}", userId);
            }

        }


        public async Task<ShoppingCartDto> GetCartAsync(int userId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                return null;
            }

            var cartDto = _mapper.Map<ShoppingCartDto>(cart);

            return cartDto;
        }


        public async Task RemoveFromCartAsync(int itemId)
        {
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync;
        }

        public Task UpdateQuantityAsync(int itemId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
