using RandomApp.ProductManagement.Application.DataTransferObjects;

namespace RandomApp.Web.Client;

public interface IProductDisplayService
{
    public Task<IEnumerable<ProductDto>> GetProductsAsync();
}
