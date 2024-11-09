using Common.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Random.App.ProductManagement.Domain.Entities;
using Random.App.ProductManagement.Domain.RepositoryInterfaces;
using Random.App.ProductManagement.Infrastructure.DataAccess;

namespace Random.App.ProductManagement.Infrastructure.RepositoryImplementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ProductDbContext _productDbContext;

        public ProductRepository(ProductDbContext context) : base(context)
        {
            _productDbContext = context;
        }

        public async Task<IEnumerable<Product>> GetPopularProducts(int count)
        {
            return await _productDbContext.Products.OrderByDescending(d => d.Name).Take(count).ToListAsync();
        }
    }
}
