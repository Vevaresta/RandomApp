using Common.Shared.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Random.App.ProductManagement.Domain.Entities;
using Random.App.ProductManagement.Domain.RepositoryInterfaces;
using System.Linq.Expressions;


namespace Random.App.ProductManagement.API.Controllers
{
    //The use of the FromBody attribute to select data from the request body and explicitly check the
    // ModelState.IsValid property is not required in controllers that have been decorated with the ApiController attribute.  
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;


        public ProductController(IUnitOfWork unitOfWork, IProductRepository productRepository, ILogger<ProductController> logger)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
            _logger = logger;
        }

        // ProducesResponseType->usefull for swagger API documentation, public facing APIs and when dealing with multiple response scenarios
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById([FromRoute] int id)
        {
            _logger.LogInformation("Fetching product with ID: {id}", id);
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with ID {id} not found.", id);
                return NotFound($"Product with ID {id} not found.");
                //return StatusCode(StatusCodes.Status404NotFound, ("Product with ID {id} not found.", id));
            }

            _logger.LogInformation("Returning product with ID {id}", id);

            return Ok(product);
        }


        [HttpGet("all")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            _logger.LogInformation("Fetching all products.");
            var products = await _productRepository.GetAllAsync();

            if (products == null)
            {
                _logger.LogWarning("No products found.");
                return NotFound("No products found.");
            }

            _logger.LogInformation("Returned {Count} products.", products.Count());
            return Ok(products);
        }


        [HttpGet("find")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> FindEProducts([FromQuery] string? name, [FromQuery] string? description)
        {
            _logger.LogInformation("Fetching product with name {name} and description {description}.", name, description);

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(description))
            {
                _logger.LogWarning("No search criteria provided.");
                return BadRequest("No search criteria provided.");
            }

            Expression<Func<Product, bool>> filter = entity =>
                (!string.IsNullOrEmpty(name) && entity.Name.Contains(name)) ||
                (!string.IsNullOrEmpty(description) && entity.Description.Contains(description));

            var entities = await _productRepository.Find(filter);

            if (!entities.Any())
            {
                _logger.LogInformation("No products found matching the criteria");
                return NotFound("No products found matching the criteria");
            }

            _logger.LogInformation("Found {Count} products matching the criteria.", entities.Count());
            return Ok(entities);
        }


        [HttpPost("add")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            // Check if the model is valid (e.g., all required fields are provided and valid)
            //if (!ModelState.IsValid) 
            //{
            //    return BadRequest(ModelState);
            //}
            if (product == null)
            {
                _logger.LogWarning("Attempt to add a null product.");
                return BadRequest("Attempt to add a null product.");
            }

            _logger.LogInformation("Adding a new product with the name {product.Name}", product.Name);

            await _productRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Product {product.Name} saved to the database with ID {product.Id}.", product.Name, product.Id);

            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }


        [HttpPost("addRange")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddProducts([FromBody] IEnumerable<Product> products)
        {
            if (products == null || !products.Any())
            {
                _logger.LogWarning("Attempt to add null products or empty product list.");
                return BadRequest("Attempt to add null products or empty product list.");
            }

            _logger.LogInformation("Attempting to add products");

            await _productRepository.AddRangeAsync(products);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Products added to the database");

            return CreatedAtAction(nameof(GetAllProducts), null, products);

        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProduct([FromRoute] int id)
        {
            _logger.LogInformation("Attempting to find product with ID {id}", id);


            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                _logger.LogWarning("Product with ID {id} not found", id);
                return NotFound($"Product with ID {id} not found.");
            }

            _logger.LogWarning("Attempting to remove product with an ID {id}", id);

            _productRepository.Remove(product);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Product with an ID {id} removed succesfully.", id);

            return NoContent();
        }


        [HttpDelete("removeRange")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveProducts([FromQuery] IEnumerable<int> products)
        {
            _logger.LogInformation("Attempting to remove range of products.");

            if (products == null || !products.Any())
            {
                _logger.LogWarning("Product IDs cannot be null or empty.");
                return BadRequest("Product IDs cannot be null or empty.");
            }

            _logger.LogInformation("Fetching products for the provided IDs.");
            var productsToDelete = await _productRepository.Find(p => products.Contains(p.Id));

            if (!productsToDelete.Any())
            {
                _logger.LogWarning("No products found with the given IDs: {products}", products);
                return NotFound($"No products found with the given IDs: {string.Join(", ", products)}.");
            }

            _logger.LogInformation("Removing {Count} products.", productsToDelete.Count());

            _productRepository.RemoveRange(productsToDelete);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("Successfully removed products with IDs: {ProductIds}", string.Join(", ", products));

            return NoContent();
        }


        [HttpGet("getPopularProducts")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetPopularProducts([FromQuery] string? keyword)
        {
            _logger.LogInformation("Attempting to search for product with the keyword {keyword}", keyword);

            if (string.IsNullOrWhiteSpace(keyword))
            {
                _logger.LogWarning("Keyword cannot be null or empty");
                return BadRequest("Keyword cannot be null or empty");
            }

            _logger.LogInformation("Fetching products with the keyword {keyword}", keyword);

            var popularProducts = await _productRepository.GetPopularProducts(keyword);

            if (!popularProducts.Any())
            {
                _logger.LogInformation("No products with the keyword {keyword} found", keyword);
                return NotFound($"No products with the keyword {keyword} found");
            }

            _logger.LogInformation("Found {Count} products for the keyword: {keyword}", popularProducts.Count(), keyword);

            return Ok(new
            {
                Count = popularProducts.Count(),
                Products = popularProducts
            });
        }


    }
}