using Common.Shared.Repositories;
using Microsoft.AspNetCore.Mvc;
using NLog;
using AutoMapper;
using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;
using RandomApp.ShoppingCartManagement.Domain.Entities;
using RandomApp.ShoppingCartManagement.Domain.ValueObjects;
using RandomApp.ShoppingCartManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Domain.Entities;

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
        public async Task<IActionResult> UpdateQuantity(int userId, int productId, int quantity)
        {
            var success = await _shoppingCartDbService.UpdateQuantityAsync(productId, quantity, userId);

            if (!success)
            {
                return NotFound("Product {productId} not found in the cart or invalid quantity.");
            }

            return Ok(new { Message = "Quantity updated successfully", ProductId =  productId, NewQuantity = quantity });
        }


        [HttpDelete("remove/{productId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveFromCart(int productId, [FromQuery] int userId)
        {
            _logger.Info("Attempting to remove product {productId} from cart for user {userId}", productId, userId);
            
            bool success = await _shoppingCartDbService.RemoveFromCartAsync(userId, productId);

            if (!success)
            {
                return NotFound("Product or user not found.");
            }

            return Ok();
        }


        [HttpDelete("clear/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ClearCart(int userId)
        {
            _logger.Info("Attempting to clear cart for user {userId}", userId);
            var success = await _shoppingCartDbService.ClearCartAsync(userId);

            if (!success)
            {
                return NotFound($"User {userId} not found.");
            }

            return Ok();

        }
    }
}
