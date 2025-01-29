﻿using Common.Shared.Repositories;
using Microsoft.AspNetCore.Mvc;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;
using NLog;
using RandomApp.ShoppingCartManagement.Application.Services;
using AutoMapper;
using System.Reflection;
using RandomApp.ShoppingCartManagement.Application.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace RandomApp.ShoppingCartManagement.Application.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger _logger;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IMapper _mapper;

        public ShoppingCartController(IUnitOfWork unitOfWork, IShoppingCartRepository shoppingCartRepository, IShoppingCartService shoppingCartService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _shoppingCartRepository = shoppingCartRepository;
            _logger = LogManager.GetCurrentClassLogger();
            _shoppingCartService = shoppingCartService;
            _mapper = mapper;
        }


        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(ShoppingCartDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize] do that later
        public async Task<ActionResult<ShoppingCartDto>> GetCart(int userId)
        {
            _logger.Info("Fetching shopping cart for user: {userId}", userId);
            var cart = await _shoppingCartRepository.GetCartByItemIdAsync(userId);

            if (cart == null)
            {
                _logger.Warn("No cart with User Id {id} found.", userId);
                return NotFound($"Product with ID {userId} not found.");
            }
            _logger.Info("Returning cart with user Id {id}", userId);
            return Ok(cart);
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ShoppingCartDto>>> GetAllCarts()
        {
            var carts = await _shoppingCartRepository.GetAllAsync();

            if (carts == null)
            {
                return NotFound("No shopping carts found.");
            }

            return Ok(carts);
        }
    }
}
