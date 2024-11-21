using Common.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Random.App.ProductManagement.Domain.Entities;
using Random.App.ProductManagement.Domain.RepositoryInterfaces;
using System.Linq.Expressions;

namespace Random.App.ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;

        public ProductController(IUnitOfWork unitOfWork, IProductRepository productRepository)
        {
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _productRepository.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("find")]
        public async Task<IActionResult> FindEProducts([FromQuery] string? name, [FromQuery] string? description)
        {
            Expression<Func<Product, bool>> filter = entity =>
                (!string.IsNullOrEmpty(name) && entity.Name.Contains(name)) ||
                (!string.IsNullOrEmpty(description) && entity.Description.Contains(description));

            var entities = await _productRepository.Find(filter);

            return Ok(entities);
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddProduct([FromBody] Product product)
        {
            await _productRepository.AddAsync(product);
            await _unitOfWork.CompleteAsync();
            return Ok(product);
        }

        [HttpPost("addRange")]
        public async Task<IActionResult> AddProducts([FromBody] IEnumerable<Product> products)
        {
            await _productRepository.AddRangeAsync(products);
            await _unitOfWork.CompleteAsync();
            return Ok(products);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult>RemoveProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);

            if(product == null)
            {
                return NotFound($"Product with ID {id} not found");
            }

            _productRepository.Remove(product);
            await _unitOfWork.CompleteAsync();

            return Ok(product);
        }


        [HttpDelete("removeRange")]
        public async Task<IActionResult> RemoveProducts([FromBody]IEnumerable<int> products)
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

            return Ok();
        }

    }
}
