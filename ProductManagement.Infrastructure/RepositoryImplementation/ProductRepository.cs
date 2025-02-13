using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;
using RandomApp.ProductManagement.Infrastructure.DataAccess;

namespace RandomApp.ProductManagement.Infrastructure.RepositoryImplementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ProductDbContext _productDbContext;

        public ProductRepository(ProductDbContext context) : base(context)
        {
            _productDbContext = context;
        }

        public async Task<IEnumerable<Product>> GetPopularProducts(string keyword)
        {
            return await _productDbContext.Products
                .Where(p => p.ProductDescription.Value.Contains(keyword))
                .ToListAsync();

        }

        public async Task<Product> GetProductByApiIdAsync(int originalApiId)
        {
            return await _productDbContext.Products.FirstOrDefaultAsync(p => p.OriginalApiId == originalApiId);
        }
    }
}
