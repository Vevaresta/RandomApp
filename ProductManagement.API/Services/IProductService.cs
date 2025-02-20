using RandomApp.ProductManagement.Application.DataTransferObjects;

namespace RandomApp.ProductManagement.Application.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProductsFromApiAsync();
    }
}
