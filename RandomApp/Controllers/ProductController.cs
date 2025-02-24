using Common.Shared.Authorization;
using Common.Shared.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Domain.Models;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;
using System.Linq.Expressions;


namespace RandomApp.Presentation.Api.Controllers
{

    // ModelState.IsValid property is not required in controllers that have been decorated with the ApiController attribute.  
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly NLog.ILogger _logger;
        private readonly IProductService _productService;
        private readonly IProductSyncService _productSyncService;


        public ProductController(IUnitOfWork unitOfWork, IProductRepository productRepository, IProductService productService, IProductSyncService productSyncService)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
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
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.Warn("Product with ID {id} not found.", id);
                return NotFound($"Product with ID {id} not found.");
                //return StatusCode(StatusCodes.Status404NotFound, ("Product with ID {id} not found.", id));
            }

            _logger.Info("Returning product with ID {id}", id);

            return Ok(product);
        }


        [HttpGet("all")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            _logger.Info("Fetching all products.");
            var products = await _productRepository.GetAllAsync();

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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindEProducts(string? name, string? description)
        {
            _logger.Info("Fetching product with name {name} and description {description}.", name, description);

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(description))
            {
                _logger.Warn("No search criteria provided.");
                return BadRequest("No search criteria provided.");
            }

            Expression<Func<Product, bool>> filter = entity =>
                !string.IsNullOrEmpty(name) && entity.Name.Contains(name) ||
                !string.IsNullOrEmpty(description) && entity.ProductDescription.ToString().Contains(description);

            var entities = await _productRepository.Find(filter);

            if (!entities.Any())
            {
                _logger.Info("No products found matching the criteria");
                return NotFound("No products found matching the criteria");
            }

            _logger.Info("Found {Count} products matching the criteria.", entities.Count());
            return Ok(entities);
        }


        [HttpPost("add")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct(Product product)
        {
            // Check if the model is valid (e.g., all required fields are provided and valid) you don't have to write this if you activate [ApiController] it does it automatically
            //if (!ModelState.IsValid) 
            //{
            //    return BadRequest(ModelState);
            //}
            if (product == null)
            {
                _logger.Warn("Attempt to add a null product.");
                return BadRequest("Attempt to add a null product.");
            }

            _logger.Info("Adding a new product with the name {product.Name}", product.Name);

            await _productRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            _logger.Info("Product {product.Name} saved to the database with ID {product.Id}.", product.Name, product.Id);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }


        [HttpPost("addRange")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProducts(IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
            {
                _logger.Warn("Attempt to add null products or empty product list.");
                return BadRequest("Attempt to add null products or empty product list.");
            }

            _logger.Info("Attempting to add products");

            await _productRepository.AddRangeAsync(products);
            await _unitOfWork.CompleteAsync();

            _logger.Info("Products added to the database");

            return CreatedAtAction(nameof(GetAllProducts), null, products);

        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            _logger.Info("Attempting to find product with ID {id}", id);


            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.Warn("Product with ID {id} not found", id);
                return NotFound($"Product with ID {id} not found.");
            }

            _logger.Warn("Attempting to remove product with an ID {id}", id);

            _productRepository.Remove(product);
            await _unitOfWork.CompleteAsync();

            _logger.Info("Product with an ID {id} removed succesfully.", id);

            return NoContent();
        }


        [HttpDelete("removeRange")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProducts(IEnumerable<int> products)
        {
            _logger.Info("Attempting to remove range of products.");

            if (products == null || !products.Any())
            {
                _logger.Warn("Product IDs cannot be null or empty.");
                return BadRequest("Product IDs cannot be null or empty.");
            }

            _logger.Info("Fetching products for the provided IDs.");
            var productsToDelete = await _productRepository.Find(p => products.ToList().Contains(p.Id.GetHashCode()));

            if (!productsToDelete.Any())
            {
                _logger.Warn("No products found with the given IDs: {products}", products);
                return NotFound($"No products found with the given IDs: {string.Join(", ", products)}.");
            }

            _logger.Info("Removing {Count} products.", productsToDelete.Count());

            _productRepository.RemoveRange(productsToDelete);
            await _unitOfWork.CompleteAsync();

            _logger.Info("Successfully removed products with IDs: {ProductIds}", string.Join(", ", products));

            return NoContent();
        }


        [HttpGet("getPopularProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPopularProducts(string? keyword)
        {
            _logger.Info("Attempting to search for product with the keyword {keyword}", keyword);

            if (string.IsNullOrWhiteSpace(keyword))
            {
                _logger.Warn("Keyword cannot be null or empty");
                return BadRequest("Keyword cannot be null or empty");
            }

            _logger.Info("Fetching products with the keyword {keyword}", keyword);

            var popularProducts = await _productRepository.GetPopularProducts(keyword);

            if (!popularProducts.Any())
            {
                _logger.Info("No products with the keyword {keyword} found", keyword);
                return NotFound($"No products with the keyword {keyword} found");
            }

            _logger.Info("Found {Count} products for the keyword: {keyword}", popularProducts.Count(), keyword);

            return Ok(new
            {
                Count = popularProducts.Count(),
                Products = popularProducts
            });
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