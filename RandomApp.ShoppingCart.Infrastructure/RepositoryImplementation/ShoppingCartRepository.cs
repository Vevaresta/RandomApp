using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using RandomApp.ShoppingCart.Domain.RepositoryInterfaces;
using RandomApp.ShoppingCart.Infrastructure.DataAccess;
using System.Linq.Expressions;

namespace RandomApp.ShoppingCart.Infrastructure.RepositoryImplementation
{
    public class ShoppingCartRepository : GenericRepository<Domain.Entities.ShoppingCart>, IShoppingCartRepository
    {
        private readonly ShoppingCartDbContext _shoppingCartDbContext;


        public ShoppingCartRepository(ShoppingCartDbContext context) : base(context)
        {
            _shoppingCartDbContext = context;
        }
    }
}
