using Moq;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;


namespace RandomApp.ShoppingCartManagement.Application.Services
{
    public static class MockShoppingCartService
    {
        public static Mock<IShoppingCartRepository> CreateMockShoppingCartRepository()
        {
            var mockCarts = new List<ShoppingCart>
        {
            new ShoppingCart
            {
                Id = 1,
                UserId = 1,
                CreatedAt = DateTime.UtcNow,
                Items = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        Id = 1,
                        ProductId = 1,
                        Name = "iPhone 14 Pro",
                        Price = 999.99M,
                        Quantity = 1
                    }
                }
            }
        };

            var mockRepo = new Mock<IShoppingCartRepository>();
            mockRepo.Setup(x => x.GetCartByUserIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int userId) => mockCarts.FirstOrDefault(c => c.UserId == userId));

            mockRepo.Setup(x => x.GetAllAsync()).ReturnsAsync(mockCarts);

            return mockRepo;
        }
    }
}
