using Common.Infrastructure.Repositories;
using Random.App.ProductManagement.Domain.Entities;

namespace Random.App.ProductManagement.Domain.RepositoryInterfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetPopularProducts(string keyword);
    }
}
