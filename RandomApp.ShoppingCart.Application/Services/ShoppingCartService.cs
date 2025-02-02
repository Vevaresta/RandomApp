using AutoMapper;
using Common.Shared.Repositories;
using NLog;
using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;
using RandomApp.ShoppingCartManagement.Application.Services.Interfaces;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;

namespace RandomApp.ShoppingCartManagement.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartService(IMapper mapper, IShoppingCartRepository shoppingCartRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = LogManager.GetCurrentClassLogger();
            _shoppingCartRepository = shoppingCartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddToCartAsync(ShoppingCartItemDto itemDto, int userId)
        {
            if (itemDto != null && itemDto.Quantity > 0)
            {
                _logger.Info("Fetching cart for user {userId}", userId);
                var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

                if (cart == null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow,
                        Items = new List<ShoppingCartItem>()
                    };
                    _logger.Info("Created new cart for user {userId}", userId);
                    await _shoppingCartRepository.AddAsync(cart);
                }

                var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == itemDto.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += itemDto.Quantity;
                    _logger.Info("Updated quantity for item {productId} in cart. New quantity: {quantity}",
                        itemDto.ProductId, existingItem.Quantity);
                }
                else
                {
                    cart.Items.Add(new ShoppingCartItem
                    {
                        ProductId = itemDto.ProductId,
                        Name = itemDto.Name,
                        Price = itemDto.Price,
                        Quantity = itemDto.Quantity,
                        Image = itemDto.Image
                    });
                    _logger.Info("Added new item {productId} to cart", itemDto.ProductId);
                }

                await _unitOfWork.CompleteAsync();
                _logger.Info("Saved changes to cart for user {userId}", userId);
            }
            else
            {
                _logger.Warn("Invalid item or quantity for user {userId}", userId);
            }
        }

        public async Task ClearCartAsync(int userId)
        {
            _logger.Info("Attempting to clear cart for user {userId}", userId);

            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.Warn("No cart found for user {userId}", userId);
                return;
            }

            cart.Items.Clear();
            await _unitOfWork.CompleteAsync();
            _logger.Info("Successfully cleared cart for user {userId}", userId);
        }

        public async Task<ShoppingCartDto> GetCartAsync(int userId)
        {
            _logger.Info("Fetching cart with user id {id}", userId);
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.Warn("Cart with user id {id} doesn't exist.", userId);
                return null;
            }

            var shoppingCartDto = _mapper.Map<ShoppingCartDto>(cart);
            _logger.Info("Cart with user id {id} successfully fetched with {itemCount} items", userId, shoppingCartDto.Items.ToList().Count);

            return shoppingCartDto;
            
        }

        public async Task RemoveFromCartAsync(int itemId)
        {
            _logger.Info("Attempting to remove item {id}", itemId);

            var cart = await _shoppingCartRepository.GetCartByItemIdAsync(itemId);
            if (cart == null)
            {
                _logger.Warn("No cart found containing item {id}", itemId);
                return;
            }

            var itemToRemove = cart.Items.FirstOrDefault(item => item.Id == itemId);

            if (itemToRemove == null)
            {
                cart.Items.Remove(itemToRemove);
                await _unitOfWork.CompleteAsync();
                _logger.Info("Item {itemId} successfully removed from cart {cartId}", itemId, cart.Id);
            }

            else
            {
                _logger.Warn("Item {itemId} not found in cart {cartId}", itemId, cart.Id);
            }

        }

        public Task UpdateQuantityAsync(int itemId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
