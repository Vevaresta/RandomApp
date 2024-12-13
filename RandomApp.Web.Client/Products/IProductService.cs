using Random.App.ProductManagement.Domain.Entities;

namespace RandomApp.Web.Client.Products
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsFromApiAsync();


    }
}
