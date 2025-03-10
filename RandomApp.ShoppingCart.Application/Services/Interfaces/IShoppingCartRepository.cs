using Common.Shared.Repositories;
using RandomApp.ShoppingCartManagement.Domain.Entities;

namespace RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        Task<ShoppingCart> GetCartByUserIdAsync(int userId);

        Task<ShoppingCart> GetCartByItemIdAsync(int itemId);
    }
}
