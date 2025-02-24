using RandomApp.ProductManagement.Application.DataTransferObjects;

namespace RandomApp.ProductManagement.Application.Services.Interfaces
{
    public interface IProductQueryService
    {
        public Task<ProductDto> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    }
}
