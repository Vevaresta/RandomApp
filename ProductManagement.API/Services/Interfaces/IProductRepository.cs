using Common.Shared.Repositories;
using RandomApp.ProductManagement.Domain.Entities;

namespace RandomApp.ProductManagement.Application.Services.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetProductByApiIdAsync(int originalApiId);
    }
}
