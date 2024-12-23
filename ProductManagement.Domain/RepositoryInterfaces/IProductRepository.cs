using Common.Shared.Repositories;
using RandomApp.ProductManagement.Domain.Entities;

namespace RandomApp.ProductManagement.Domain.RepositoryInterfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetPopularProducts(string keyword);

        Task<Product> GetProductByApiIdAsync(int originalApiId);
    }
}
