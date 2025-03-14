using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;

namespace RandomApp.ShoppingCartManagement.Application.Services.Interfaces
{
    public interface IShoppingCartDbService
    {
        Task<ShoppingCartDto> GetCartAsync(int userId);
        Task AddToCartAsync(ShoppingCartItemDto itemDto, int userId);
        Task UpdateQuantityAsync(int productId, int quantity, int userId);
        Task RemoveFromCartAsync(int userId, int productId);
        Task ClearCartAsync(int userId);
    }
}
