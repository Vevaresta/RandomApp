using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Domain.Models;

namespace RandomApp.Web.Client.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsFromApiAsync();

    }
}
