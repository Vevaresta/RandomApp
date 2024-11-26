using Common.Infrastructure.Repositories;
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
                return NotFound();
                //return StatusCode(StatusCodes.Status404NotFound, ("Product with ID {id} not found.", id));
            }

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
                return NotFound();
            }
            return Ok(products);
        }


        [HttpGet("find")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FindEProducts([FromQuery] string? name, [FromQuery] string? description)
        {
            _logger.LogInformation("Fetching product with name {name} and description {description}.", name, description);

            if (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(description))
            {
                _logger.LogWarning("No search criteria provided.");
                return BadRequest();
            }

            try
            {
                Expression<Func<Product, bool>> filter = entity =>
                    (!string.IsNullOrEmpty(name) && entity.Name.Contains(name)) ||
                    (!string.IsNullOrEmpty(description) && entity.Description.Contains(description));

                var entities = await _productRepository.Find(filter);

                if (!entities.Any())
                {
                    _logger.LogInformation("No products found matching the criteria");
                    return NotFound();
                }

                _logger.LogInformation("Found {Count} products matching the criteria.", entities.Count());
                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching products with name '{name}' and description '{description}'.", name, description);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }

        }


        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            // Check if the model is valid (e.g., all required fields are provided and valid)
            //if (!ModelState.IsValid) 
            //{
            //    return BadRequest(ModelState);
            //}
            _logger.LogInformation("Adding a new product with the name {product}", product);
            await _productRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            _logger.LogInformation("Product {product} saved to the database.", product);
            
            return CreatedAtAction(nameof(GetProductById), new {id = product.Id}, product);
        }

        [HttpPost("addRange")]
        public async Task<IActionResult> AddProducts([FromBody] IEnumerable<Product> products)
        {
            await _productRepository.AddRangeAsync(products);
            await _unitOfWork.CompleteAsync();
            return Ok(products);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct([FromRoute] int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if(product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            _productRepository.Remove(product);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }


        [HttpDelete("removeRange")]
        public async Task<IActionResult> RemoveProducts([FromQuery]IEnumerable<int> products)
        {
            if (products == null || !products.Any())
            {
                return BadRequest("Product IDs cannot be null or empty.");
            }

            var productsToDelete = await _productRepository.Find(p => products.Contains(p.Id));

            if (!productsToDelete.Any())
            {
                return NotFound("No products found with the given IDs.");
            }

            _productRepository.RemoveRange(productsToDelete);
            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpGet("getPopularProducts")]
        public async Task<IActionResult> GetPopularProducts([FromQuery] string? keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return BadRequest("Keyword cannot be null or empty");
            }

            var popularProducts = await _productRepository.GetPopularProducts(keyword);

            if (!popularProducts.Any())
            {
                return NotFound($"No products with the keyword {keyword} found");
            }

            var count = popularProducts.Count();
            return Ok(count);
            
        }
        

    }
}