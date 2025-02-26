using Common.Shared.Authorization;
using Common.Shared.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Domain.Models;

namespace RandomApp.Presentation.Api.Controllers
{

    // ModelState.IsValid property is not required in controllers that have been decorated with the ApiController attribute.  
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductDbService _productDbService;
        private readonly NLog.ILogger _logger;
        private readonly IProductService _productService;
        private readonly IProductSyncService _productSyncService;


        public ProductController(IUnitOfWork unitOfWork, IProductDbService productDbService, IProductService productService, IProductSyncService productSyncService)
        {
            _unitOfWork = unitOfWork;
            _productDbService = productDbService;
            _logger = LogManager.GetCurrentClassLogger();
            _productService = productService;
            _productSyncService = productSyncService;
        }

        // ProducesResponseType->usefull for swagger API documentation, public facing APIs and when dealing with multiple response scenarios
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = Policies.RequireAdminPolicy)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            _logger.Info("Fetching product with ID: {id}", id);
            var product = await _productDbService.GetProductByIdAsync(id);

            if (product == null)
            {
                _logger.Warn("Product with ID {id} not found.", id);
                return NotFound($"Product with ID {id} not found.");
            }

            _logger.Info("Returning product with ID {id}", id);

            return Ok(product);
        }


        [HttpGet("all")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProducts()
        {
            _logger.Info("Fetching all products.");
            var products = await _productDbService.GetAllProductsAsync();

            if (products == null)
            {
                _logger.Warn("No products found.");
                return NotFound("No products found.");
            }

            _logger.Info("Returned {Count} products.", products.Count());
            return Ok(products);
        }


        [HttpGet("find")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> FindProducts([FromQuery] string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                _logger.Warn("Search attempted with empty search term");
                return BadRequest("Search term is required");
            }

            var products = await _productDbService.FindByNameOrDescription(searchTerm);
            _logger.Info("Search completed for term {searchTerm}. Found {Count} results.", searchTerm, products.Count());
            return Ok(products);
        }


        [HttpPost("add")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            if (productDto == null)
            {
                _logger.Warn("Attempt to add a null product.");
                return BadRequest("Attempt to add a null product.");
            }

            _logger.Info("Adding a new product with the name {product.Name}", productDto.Name);
            var savedProductDto = await _productDbService.AddAsync(productDto);
            
            _logger.Info("Product {product.Name} successfully created with ID {product.Id}.", savedProductDto.Name, savedProductDto.Id);

            return CreatedAtAction(nameof(GetProductById), new { id = savedProductDto.Id }, savedProductDto);
        }


        [HttpPost("addRange")]
        [ProducesResponseType(typeof(IEnumerable<ProductDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProducts(IEnumerable<ProductDto> productDtos)
        {
            if (productDtos == null || !productDtos.Any())
            {
                _logger.Warn("Attempt to add null products or empty product list.");
                return BadRequest("Attempt to add null products or empty product list.");
            }

            _logger.Info("Attempting to add products");

            var savedProductDtos = await _productDbService.AddRangeAsync(productDtos);

            _logger.Info("Products added to the database");

            return CreatedAtAction(nameof(GetAllProducts), null, savedProductDtos);

        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            _logger.Info("Attempting to remove product with an ID {id}", id);
            var success = await _productDbService.Remove(id);

            if (success)
            {
                _logger.Info("Product with an ID {id} removed succesfully", id);
                return NoContent();
            }
            return NotFound();
        }


        [HttpDelete("removeRange")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProducts(IEnumerable<ProductDto> products)
        {
            _logger.Info("Attempting to remove range of products.");

            var success = await _productDbService.RemoveRange(products);

            if (success)
            {
                _logger.Info("Products removed successfully");
                return NoContent();
            }

            return NotFound();
        }


        [HttpGet("sync/status")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<ProductSyncStatus?> GetProductSyncStatus()
        {
            _logger.Info("Product status finding initiated");
            var status = _productSyncService.CurrentSyncStatus;
            return Ok(new
            {
                status,
                message = "Current sync status retrieved"
            });
        }


        [HttpGet("sync/initiate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> InitProductSync()
        {
            if (_productSyncService.CurrentSyncStatus?.IsSyncRunning == true)
            {
                return Conflict(new
                {
                    message = "Sync is already in progress",
                    status = _productSyncService.CurrentSyncStatus
                });
            }

            var result = await _productSyncService.InitiateSyncAsync();

            return Ok(new
            {
                message = result.Success ? "Sync compleed successfully" : "Sync failed",
                status = _productSyncService.CurrentSyncStatus,
                result
            });
        }

    }
}