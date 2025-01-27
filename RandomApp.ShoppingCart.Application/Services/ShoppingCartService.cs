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
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                cart = new ShoppingCart()
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    Items = new List<ShoppingCartItem>()
                };

                await _shoppingCartRepository.AddAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == itemDto.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += itemDto.Quantity;
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
            await _unitOfWork.CompleteAsync();
        }


        public Task ClearCartAsync(int userId)
        {
            throw new NotImplementedException();
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
