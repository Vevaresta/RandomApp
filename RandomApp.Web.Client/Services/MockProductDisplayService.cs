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
                Name = "iPhone 14 Pro",
                Amount = 100.99M,
                Category = "Electronics",
                ProductDescription = "Latest iPhone model with advanced camera system and A16 Bionic chipggggggggggggggggggggggggggggggggggggggggg",
                Image = "https://placeholder.com/iphone14pro.jpg"
            },
            new ProductDto
            {
                Id = 2,
                Name = "Samsung 4K Smart TV",
                Amount = 799.99M,
                Category = "Electronics",
                ProductDescription = "55-inch 4K Ultra HD Smart LED TV with HDR",
                Image = "https://placeholder.com/samsungtv.jpg"
            },
            new ProductDto
            {
                Id = 3,
                Name = "Nike Air Max",
                Amount = 129.99M,
                Category = "Footwear",
                ProductDescription = "Comfortable running shoes with Air cushioning",
                Image = "https://placeholder.com/nikeairmax.jpg"
            },
            new ProductDto
            {
                Id = 4,
                Name = "MacBook Air M2",
                Amount = 1299.99M,
                Category = "Electronics",
                ProductDescription = "Lightweight laptop with M2 chip, 13.6-inch Liquid Retina display and up to 18 hours of battery life",
                Image = "https://placeholder.com/macbookair.jpg"
            },
            new ProductDto
            {
                Id = 5,
                Name = "Sony WH-1000XM4",
                Amount = 349.99M,
                Category = "Electronics",
                ProductDescription = "Premium noise-cancelling headphones with exceptional sound quality and 30-hour battery life",
                Image = "https://placeholder.com/sonywh1000xm4.jpg"
            },
            new ProductDto
            {
                Id = 6,
                Name = "Adidas Ultraboost",
                Amount = 179.99M,
                Category = "Footwear",
                ProductDescription = "Performance running shoes with responsive Boost midsole and Primeknit upper",
                Image = "https://placeholder.com/ultraboost.jpg"
            },
            new ProductDto
            {
                Id = 7,
                Name = "Canon EOS R6",
                Amount = 2499.99M,
                Category = "Electronics",
                ProductDescription = "Full-frame mirrorless camera with 20MP sensor, 4K video, and advanced autofocus",
                Image = "https://placeholder.com/canonr6.jpg"
            },
            new ProductDto
            {
                Id = 8,
                Name = "Dell XPS 15",
                Amount = 1799.99M,
                Category = "Electronics",
                ProductDescription = "Premium laptop with 15.6-inch 4K display, Intel i9 processor, and NVIDIA RTX graphics",
                Image = "https://placeholder.com/dellxps15.jpg"
            },
            new ProductDto
            {
                Id = 9,
                Name = "Puma RS-X",
                Amount = 109.99M,
                Category = "Footwear",
                ProductDescription = "Retro-style sneakers with modern cushioning technology and bold design",
                Image = "https://placeholder.com/pumarsx.jpg"
            },
            new ProductDto
            {
                Id = 10,
                Name = "iPad Pro 12.9",
                Amount = 1099.99M,
                Category = "Electronics",
                ProductDescription = "Powerful tablet with M2 chip, Liquid Retina XDR display, and Apple Pencil support",
                Image = "https://placeholder.com/ipadpro.jpg"
            },
            new ProductDto
            {
                Id = 11,
                Name = "New Balance 990v5",
                Amount = 184.99M,
                Category = "Footwear",
                ProductDescription = "Classic running shoes made in USA with ENCAP midsole technology for support",
                Image = "https://placeholder.com/newbalance990.jpg"
            },
            new ProductDto
            {
                Id = 12,
                Name = "Samsung Galaxy S23 Ultra",
                Amount = 1199.99M,
                Category = "Electronics",
                ProductDescription = "Premium smartphone with 200MP camera, S Pen support, and Snapdragon 8 Gen 2",
                Image = "https://placeholder.com/s23ultra.jpg"
            },
            new ProductDto
            {
                Id = 13,
                Name = "Reebok Classic Leather",
                Amount = 79.99M,
                Category = "Footwear",
                ProductDescription = "Timeless sneakers with soft leather upper and EVA midsole for cushioning",
                Image = "https://placeholder.com/reebokclassic.jpg"
            },
            new ProductDto
            {
                Id = 14,
                Name = "Reebok Classic Leather",
                Amount = 79.99M,
                Category = "Footwear",
                ProductDescription = "Timeless sneakers with soft leather upper and EVA midsole for cushioning",
                Image = "https://placeholder.com/reebokclassic.jpg"
            },
            new ProductDto
            {
                Id = 15,
                Name = "Reebok Classic Leather",
                Amount = 79.99M,
                Category = "Footwear",
                ProductDescription = "Timeless sneakers with soft leather upper and EVA midsole for cushioning",
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