using RandomApp.ProductManagement.Application.DataTransferObjects;

namespace RandomApp.Web.Client;

public interface IProductDisplayService
{
    public Task<IEnumerable<ProductDto>> GetProductsAsync();

    public Task<ProductDto> GetProductByIdAsync(int Id);
}
