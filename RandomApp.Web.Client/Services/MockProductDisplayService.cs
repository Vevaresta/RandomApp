using Moq;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.Web.Client;

public class MockProductDisplayService
{
    public static IProductDisplayService CreateMockProductService()
    {
        var mockProducts = new List<ProductDto>
        {
            new ProductDto
            {
                Id = 1,
                Title = "iPhone 14 Pro",
                Price = 999.99,
                Category = "Electronics",
                Description = "Latest iPhone model with advanced camera system and A16 Bionic chipggggggggggggggggggggggggggggggggggggggggg",
                Image = "https://placeholder.com/iphone14pro.jpg"
            },
            new ProductDto
            {
                Id = 2,
                Title = "Samsung 4K Smart TV",
                Price = 799.99,
                Category = "Electronics",
                Description = "55-inch 4K Ultra HD Smart LED TV with HDR",
                Image = "https://placeholder.com/samsungtv.jpg"
            },
            new ProductDto
            {
                Id = 3,
                Title = "Nike Air Max",
                Price = 129.99,
                Category = "Footwear",
                Description = "Comfortable running shoes with Air cushioning",
                Image = "https://placeholder.com/nikeairmax.jpg"
            }
        };

        // Create and setup the mock service
        var mockService = new Mock<IProductDisplayService>();
        mockService.Setup(x => x.GetProductsAsync()).ReturnsAsync(mockProducts);

        mockService.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => mockProducts.FirstOrDefault(p => p.Id == id));

        return mockService.Object;
    }
}