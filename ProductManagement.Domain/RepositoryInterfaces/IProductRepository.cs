using Common.Infrastructure.Repositories;
using ProductManagement.Domain.Entities;

namespace ProductManagement.Domain.RepositoryInterfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetPopularProducts(int count);
    }
}
