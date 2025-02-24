using RandomApp.ProductManagement.Application.DataTransferObjects;

namespace RandomApp.ProductManagement.Application.Services.Interfaces
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDto>> GetProductsFromApiAsync();
    }
}
