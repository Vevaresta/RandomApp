﻿using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;

namespace RandomApp.ShoppingCartManagement.Application.Services.Interfaces
{
    public interface IShoppingCartService
    {
        Task<ShoppingCartDto> GetCartAsync(int userId);
        Task AddToCartAsync(ShoppingCartItemDto itemDto, int userId);
        Task UpdateQuantityAsync(int itemId, int quantity);
        Task RemoveFromCartAsync(int itemId);
        Task ClearCartAsync(int userId);
    }
}
