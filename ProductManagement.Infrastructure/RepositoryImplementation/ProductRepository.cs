using Common.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Domain.Entities;
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

        public async Task<Product> GetProductByApiIdAsync(int originalApiId)
        {
            return await _productDbContext.Products.FirstOrDefaultAsync(p => p.OriginalApiId == originalApiId);
        }
    }
}
