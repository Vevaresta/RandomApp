using RandomApp.ProductManagement.Application.DataTransferObjects;
using NLog;
using AutoMapper;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;

namespace RandomApp.Web.Client.Services
{
    public class ProductDisplayService : IProductDisplayService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public ProductDisplayService(IProductRepository productRepository, ILogger logger, IMapper mapper) 
        {
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
            
        }
        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productsDtos = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productsDtos;
        }
    }
}
