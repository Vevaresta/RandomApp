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
            },
            new ProductDto
            {
                Id = 4,
                Title = "MacBook Air M2",
                Price = 1299.99,
                Category = "Electronics",
                Description = "Lightweight laptop with M2 chip, 13.6-inch Liquid Retina display and up to 18 hours of battery life",
                Image = "https://placeholder.com/macbookair.jpg"
            },
            new ProductDto
            {
                Id = 5,
                Title = "Sony WH-1000XM4",
                Price = 349.99,
                Category = "Electronics",
                Description = "Premium noise-cancelling headphones with exceptional sound quality and 30-hour battery life",
                Image = "https://placeholder.com/sonywh1000xm4.jpg"
            },
            new ProductDto
            {
                Id = 6,
                Title = "Adidas Ultraboost",
                Price = 179.99,
                Category = "Footwear",
                Description = "Performance running shoes with responsive Boost midsole and Primeknit upper",
                Image = "https://placeholder.com/ultraboost.jpg"
            },
            new ProductDto
            {
                Id = 7,
                Title = "Canon EOS R6",
                Price = 2499.99,
                Category = "Electronics",
                Description = "Full-frame mirrorless camera with 20MP sensor, 4K video, and advanced autofocus",
                Image = "https://placeholder.com/canonr6.jpg"
            },
            new ProductDto
            {
                Id = 8,
                Title = "Dell XPS 15",
                Price = 1799.99,
                Category = "Electronics",
                Description = "Premium laptop with 15.6-inch 4K display, Intel i9 processor, and NVIDIA RTX graphics",
                Image = "https://placeholder.com/dellxps15.jpg"
            },
            new ProductDto
            {
                Id = 9,
                Title = "Puma RS-X",
                Price = 109.99,
                Category = "Footwear",
                Description = "Retro-style sneakers with modern cushioning technology and bold design",
                Image = "https://placeholder.com/pumarsx.jpg"
            },
            new ProductDto
            {
                Id = 10,
                Title = "iPad Pro 12.9",
                Price = 1099.99,
                Category = "Electronics",
                Description = "Powerful tablet with M2 chip, Liquid Retina XDR display, and Apple Pencil support",
                Image = "https://placeholder.com/ipadpro.jpg"
            },
            new ProductDto
            {
                Id = 11,
                Title = "New Balance 990v5",
                Price = 184.99,
                Category = "Footwear",
                Description = "Classic running shoes made in USA with ENCAP midsole technology for support",
                Image = "https://placeholder.com/newbalance990.jpg"
            },
            new ProductDto
            {
                Id = 12,
                Title = "Samsung Galaxy S23 Ultra",
                Price = 1199.99,
                Category = "Electronics",
                Description = "Premium smartphone with 200MP camera, S Pen support, and Snapdragon 8 Gen 2",
                Image = "https://placeholder.com/s23ultra.jpg"
            },
            new ProductDto
            {
                Id = 13,
                Title = "Reebok Classic Leather",
                Price = 79.99,
                Category = "Footwear",
                Description = "Timeless sneakers with soft leather upper and EVA midsole for cushioning",
                Image = "https://placeholder.com/reebokclassic.jpg"
            },
            new ProductDto
            {
                Id = 14,
                Title = "Reebok Classic Leather",
                Price = 79.99,
                Category = "Footwear",
                Description = "Timeless sneakers with soft leather upper and EVA midsole for cushioning",
                Image = "https://placeholder.com/reebokclassic.jpg"
            },
            new ProductDto
            {
                Id = 15,
                Title = "Reebok Classic Leather",
                Price = 79.99,
                Category = "Footwear",
                Description = "Timeless sneakers with soft leather upper and EVA midsole for cushioning",
                Image = "https://placeholder.com/reebokclassic.jpg"
            },

        };

        // Create and setup the mock service
        var mockService = new Mock<IProductDisplayService>();
        mockService.Setup(x => x.GetProductsAsync()).ReturnsAsync(mockProducts);

        mockService.Setup(x => x.GetProductByIdAsync(It.IsAny<int>())).ReturnsAsync((int id) => mockProducts.FirstOrDefault(p => p.Id == id));

        return mockService.Object;
    }
}