using Common.Shared.Repositories;
using Microsoft.AspNetCore.Mvc;
using RandomApp.ShoppingCartManagement.Domain.RepositoryInterfaces;
using NLog;
using RandomApp.ShoppingCartManagement.Application.Services;
using AutoMapper;

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

        public ShoppingCartController()
        {
        }
    }
}
