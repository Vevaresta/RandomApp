using RandomApp.ProductManagement.Application.DataTransferObjects;

namespace RandomApp.ProductManagement.Application.Services.Interfaces
{
    public interface IProductDbService
    {
        Task<ProductDto> GetProductByIdAsync(int productId);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> FindByNameOrDescription(string searchTerm);

        Task<ProductDto> AddAsync(ProductDto productDto);

        Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<ProductDto> productDto);

        Task<bool> Remove(int productId);
    }
}
