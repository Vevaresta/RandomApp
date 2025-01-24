using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using RandomApp.ShoppingCart.Domain.RepositoryInterfaces;
using RandomApp.ShoppingCartManagement.Infrastructure.DataAccess;
using System.Linq.Expressions;

namespace RandomApp.ShoppingCartManagement.Infrastructure.RepositoryImplementation
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
