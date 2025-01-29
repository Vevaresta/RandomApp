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

        public Task ClearCartAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCartDto> GetCartAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromCartAsync(int itemId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateQuantityAsync(int itemId, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
