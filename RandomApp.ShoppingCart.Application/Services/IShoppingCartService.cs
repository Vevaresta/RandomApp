using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;

namespace RandomApp.ShoppingCartManagement.Application.Services
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDto> GetCartAsync(int userId);
        Task AddToCartAsync(ShoppingCartItemDto itemDto);
        Task UpdateQuantityAsync(int itemId, int quantity);
        Task RemoveFromCartAsync(int itemId);
        Task ClearCartAsync(int userId);
    }
}
