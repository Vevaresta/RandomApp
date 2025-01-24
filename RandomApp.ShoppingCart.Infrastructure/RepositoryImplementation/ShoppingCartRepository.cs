using Common.Shared.Repositories;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;
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
    }
}
