﻿using AutoMapper;
using Common.Shared.Repositories;
using NLog;
using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;
using RandomApp.ShoppingCartManagement.Application.Services.Interfaces;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;
using RandomApp.ShoppingCartManagement.Domain.ValueObjects;

namespace RandomApp.ShoppingCartManagement.Application.Services
{
    public class ShoppingCartDbService : IShoppingCartService
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartDbService(IMapper mapper, IShoppingCartRepository shoppingCartRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _logger = LogManager.GetCurrentClassLogger();
            _shoppingCartRepository = shoppingCartRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task AddToCartAsync(ShoppingCartItemDto itemDto, int userId)
        {
            if (itemDto == null || itemDto.Quantity <= 0)
            {
                _logger.Warn("Invalid item or quantity for user {userId}", userId);
                return;
            }

            _logger.Info("Fetching cart for user {userId}", userId);
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.Info("Creating new cart for user {userId}", userId);
                cart = ShoppingCart.Create(userId);
                await _shoppingCartRepository.AddAsync(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(item => item.ProductId == itemDto.ProductId);
            if (existingItem == null)
            {
                _logger.Info("Item {productId} already exist in cart for user {userId}. Updating quantity.", itemDto.ProductId, userId);
                cart.UpdateItemQuantity(
                    existingItem.ProductId,
                    existingItem.Name,
                    existingItem.Quantity + itemDto.Quantity,
                    existingItem.Price,
                    existingItem.Image
                    );
            }
            else
            {

                var cartItem = _mapper.Map<ShoppingCartItem>(itemDto);

                cart.AddItem(
                    cartItem.ProductId,
                    cartItem.Name,
                    cartItem.Quantity,
                    cartItem.Price,
                    cartItem.Image
                    );
                _logger.Info("Updated cart for product {productId}", itemDto.ProductId);

            }

            _shoppingCartRepository.Update(cart);
            await _unitOfWork.CompleteAsync();
            _logger.Info("Saved changes to cart for user {userId}", userId);
        }



        public async Task ClearCartAsync(int userId)
        {
            _logger.Info("Attempting to clear cart for user {userId}", userId);

            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                _logger.Warn("No cart found for user {userId}", userId);
                return;
            }

            cart.Clear();
            await _unitOfWork.CompleteAsync();
            _logger.Info("Successfully cleared cart for user {userId}", userId);
        }

        public async Task<ShoppingCartDto> GetCartAsync(int userId)
        {
            _logger.Info("Fetching cart with user id {userId}", userId);
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.Warn("Cart with user id {userId} doesn't exist.", userId);
                return null;
            }

            var shoppingCartDto = _mapper.Map<ShoppingCartDto>(cart);
            if (shoppingCartDto == null || shoppingCartDto.Items == null)
            {
                _logger.Warn("Cart mapping returned null or empty for user {userId}", userId);
                return null;
            }
            _logger.Info("Cart with user id {userId} successfully fetched with {itemCount} items", userId, shoppingCartDto.Items.Count());

            return shoppingCartDto;

        }

        public async Task RemoveFromCartAsync(int userId, int productId)
        {
            _logger.Info("Attempting to remove product {productId} from cart for user {userId}", productId, userId);

            var cart = await _shoppingCartRepository.GetCartByItemIdAsync(userId);

            if (cart == null)
            {
                _logger.Warn("No cart found containing item {productId}", productId);
                return;
            }

            var item = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (item == null)
            {
                _logger.Warn("Item {productId} not found in cart for user {userId}", productId, userId);
                return;
            }

            cart.RemoveItem(productId);
            _shoppingCartRepository.Update(cart);
            await _unitOfWork.CompleteAsync();
            _logger.Info("Item {productId} removed from cart for user {userId}", productId, userId);

        }

        public async Task UpdateQuantityAsync(int productId, int quantity, int userId)
        {
            if (quantity <= 0)
            {
                _logger.Warn("Invalid quantity {quantity} for item {productId}", quantity, productId);
                return;
            }

            _logger.Info("Attemtping to update quantity for item {productId} to {quantity} for user {userId}", productId, quantity, userId);

            var cart = await _shoppingCartRepository.GetCartByItemIdAsync(userId);

            if (cart == null)
            {
                _logger.Warn("No cart found containing item {productId}", productId);
                return;
            }

            var item = cart.Items.FirstOrDefault(item => item.ProductId == productId);
            if (item == null)
            {
                _logger.Warn("Item {productId} not found in cart for user {userId}", productId, userId);
                return;
            }

            cart.UpdateItemQuantity(productId, item.Name, quantity, item.Price, item.Image);

            _shoppingCartRepository.Update(cart);
            await _unitOfWork.CompleteAsync();

            _logger.Info("Successfull updated quantity for item {productId} to {quantity} for user {userId}", productId, quantity, userId);
        }
    }
}
