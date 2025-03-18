using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;

namespace RandomApp.ShoppingCartManagement.Application.Services.Interfaces
{
    public interface IShoppingCartDbService
    {
        Task<ShoppingCartDto> GetCartAsync(int userId);
        Task AddToCartAsync(ShoppingCartItemDto itemDto, int userId);
        Task <bool>UpdateQuantityAsync(int productId, int quantity, int userId);
        Task <bool>RemoveFromCartAsync(int userId, int productId);
        Task <bool>ClearCartAsync(int userId);
    }
}
