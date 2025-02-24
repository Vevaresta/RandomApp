using AutoMapper;
using Common.Shared.Repositories;
using NLog;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;

namespace RandomApp.ProductManagement.Application.Services.Implementations
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IProductRepository _productRepository;        
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ProductQueryService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _logger = LogManager.GetCurrentClassLogger();
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productsDto = _mapper.Map<IList<ProductDto>>(products);
            return productsDto;
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }
    }
}
