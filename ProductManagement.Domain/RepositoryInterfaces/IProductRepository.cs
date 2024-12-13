using Common.Shared.Repositories;
using Random.App.ProductManagement.Domain.Entities;

namespace Random.App.ProductManagement.Domain.RepositoryInterfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetPopularProducts(string keyword);

        Task<Product> GetProductByApiIdAsync(int originalApiId);
    }
}
