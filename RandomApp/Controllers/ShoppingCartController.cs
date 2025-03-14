using Common.Shared.Repositories;
using Microsoft.AspNetCore.Mvc;
using NLog;
using AutoMapper;
using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.ValueObjects;
using RandomApp.ShoppingCartManagement.Application.Services.Interfaces;

namespace RandomApp.ShoppingCartManagement.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartDbService _shoppingCartDbService;
        private readonly NLog.ILogger _logger;
        private readonly IMapper _mapper;

        public ShoppingCartController(IUnitOfWork unitOfWork, IShoppingCartDbService shoppingCartDbService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _shoppingCartDbService = shoppingCartDbService;
            //_shoppingCartRepository = MockShoppingCartService.CreateMockShoppingCartRepository().Object;
            _logger = LogManager.GetCurrentClassLogger();
            _mapper = mapper;
        }


        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ShoppingCartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ShoppingCartDto>> GetCart(int userId)
        {
            _logger.Info("Fetching shopping cart for user: {userId}", userId);
            var cart = await _shoppingCartDbService.GetCartAsync(userId);

            if (cart == null)
            {
                _logger.Warn("No cart with User Id {id} found.", userId);
                return NotFound($"Cart for user ID {userId} not found.");
            }
            
            _logger.Info("Returning cart with user Id {id}", userId);

            return Ok(cart);
        }


        //[HttpGet("all")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<IEnumerable<ShoppingCartDto>>> GetAllCarts()
        //{
        //    var carts = await _shoppingCartDbService.
        //    if (carts == null)
        //    {
        //        return NotFound("No shopping carts found.");
        //    }
        //    return Ok(_mapper.Map<IEnumerable<ShoppingCartDto>>(carts));
        //}


        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddToCart(ShoppingCartItemDto itemDto, int userId)
        {
            if (itemDto.Quantity <= 0 || itemDto == null)
            {
                _logger.Warn("Invalid quantity {quantity} for product {productId}", itemDto.Quantity, itemDto.ProductId);
                return BadRequest("Invalid item or quantity");
            }

            await _shoppingCartDbService.AddToCartAsync(itemDto, userId);

            return Ok(new { Message = "Item added succesfully" });
        }


        [HttpPut("update-quantity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateQuantity(int itemId, int quantity)
        {
            if (quantity <= 0)
            {
                _logger.Warn("Invalid quantity {quantity} provided for item {itemId}", quantity, itemId);
                return BadRequest("Quantity must be greater than 0");
            }

            var cart = await _shoppingCartRepository.GetCartByItemIdAsync(itemId);
            if (cart == null)
            {
                _logger.Warn("No cart found containing item {itemId}", itemId);
                return NotFound($"Item {itemId} not found in any cart");
            }

            var item = cart.Items.FirstOrDefault(item => item.Id == itemId);
            if (item != null)
            {
                var oldQuantity = item.Quantity;
                item.Quantity = quantity;
                await _unitOfWork.CompleteAsync();
                _logger.Info("Updated item {itemId} quantity from {oldQuantity} to {quantity}",
                    itemId, oldQuantity, quantity);
                return Ok();
            }

            _logger.Warn("Item {itemId} not found in cart {cartId}", itemId, cart.Id);
            return NotFound($"Item {itemId} not found in cart");
        }


        [HttpDelete("remove/{itemId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            _logger.Info("Attempting to remove item {itemId}", itemId);
            var cart = await _shoppingCartRepository.GetCartByItemIdAsync(itemId);

            if (cart == null)
            {
                _logger.Warn("No cart found containing item {itemId}", itemId);
                return NotFound($"Item {itemId} not found in any cart");
            }

            var itemToRemove = cart.Items.FirstOrDefault(item => item.Id == itemId);
            if (itemToRemove != null)
            {
                cart.Items.Remove(itemToRemove);
                await _unitOfWork.CompleteAsync();
                _logger.Info("Item {itemId} removed from cart {cartId}", itemId, cart.Id);
                return Ok();
            }

            _logger.Warn("Item {itemId} not found in cart {cartId}", itemId, cart.Id);
            return NotFound($"Item {itemId} not found in cart");
        }


        [HttpDelete("clear/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearCart(int userId)
        {
            _logger.Info("Attempting to clear cart for user {userId}", userId);
            var cart = await _shoppingCartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.Warn("No cart found for user {userId}", userId);
                return NotFound($"No cart found for user {userId}");
            }

            cart.Items.Clear();
            await _unitOfWork.CompleteAsync();
            _logger.Info("Cleared cart for user {userId}", userId);
            return Ok();
        }
    }
}
