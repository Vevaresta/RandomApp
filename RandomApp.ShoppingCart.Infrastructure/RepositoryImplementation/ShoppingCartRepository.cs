using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using RandomApp.ShoppingCartManagement.Application.Services.Interfaces;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Infrastructure.DataAccess;

namespace RandomApp.ShoppingCartManagement.Infrastructure.RepositoryImplementation
{
    public class ShoppingCartRepository : GenericRepository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ShoppingCartDbContext _shoppingCartDbContext;


        public ShoppingCartRepository(ShoppingCartDbContext context) : base(context)
        {
            _shoppingCartDbContext = context;
        }

        public async Task<ShoppingCart> GetCartByUserIdAsync(int userId)
        {
            return await _shoppingCartDbContext.ShoppingCarts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.UserId == userId);
        }

        public async Task<ShoppingCart> GetCartByItemIdAsync(int userId, int productId)
        {
            return await _shoppingCartDbContext.ShoppingCarts
                .Include(cart => cart.Items)
                .FirstOrDefaultAsync(cart => cart.UserId == userId &&
                    cart.Items.Any(item => item.ProductId == productId));
              
        }
    }
}
