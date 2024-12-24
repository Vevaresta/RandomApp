using RandomApp.ProductManagement.Domain.Entities;

namespace RandomApp.ProductManagement.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsFromApiAsync();

    }
}
