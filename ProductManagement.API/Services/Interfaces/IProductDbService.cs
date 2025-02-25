using RandomApp.ProductManagement.Application.DataTransferObjects;

namespace RandomApp.ProductManagement.Application.Services.Interfaces
{
    public interface IProductDbService
    {
        public Task<ProductDto> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> FindByNameOrDescription(string searchTerm);

        public Task<ProductDto> AddAsync(ProductDto productDto);
    }
}
