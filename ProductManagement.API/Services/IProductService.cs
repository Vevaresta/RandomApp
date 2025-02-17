using RandomApp.ProductManagement.Application.DataTransferObjects;

namespace RandomApp.ProductManagement.Application.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsFromApiAsync();
    }
}
