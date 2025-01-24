using Common.Shared.Repositories;
using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;
using NLog;

namespace RandomApp.ShoppingCartManagement.Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public Task AddToCartAsync(ShoppingCartItemDto itemDto)
        {
            var item = _shoppingCartRepository.GetByIdAsync(itemDto.Id);
            if (item == null)
            {
                throw new ArgumentNullException(nameof(itemDto));
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
