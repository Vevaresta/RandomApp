using Common.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Domain.Entities;
using ProductManagement.Domain.RepositoryInterfaces;
using ProductManagement.Infrastructure.ORM;

namespace ProductManagement.Infrastructure.RepositoryImplementation
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ProductDbContext _context;

        public ProductRepository(ProductDbContext context) : base(context)
        {
            _context = context;  
        }

        public async Task<IEnumerable<Product>> GetPopularProducts(int count)
        {
            return await _context.Products.OrderByDescending(d => d.Name).Take(count).ToListAsync();
        }
    }
}
