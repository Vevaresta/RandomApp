using RandomApp.ProductManagement.Application.DataTransferObjects;
using AutoMapper;
using RandomApp.ProductManagement.Application.Services.Interfaces;

namespace RandomApp.Web.Client.Services
{
    public class ProductDisplayService : IProductDisplayService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductDisplayService(IProductRepository productRepository, IMapper mapper) 
        {
            _productRepository = productRepository;
            _mapper = mapper;
            
        }

        public async Task<ProductDto> GetProductByIdAsync(int Id)
        {
            var product = await _productRepository.GetByIdAsync(Id);
            var productDto = _mapper.Map<ProductDto>(product);

            return productDto;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productsDtos = _mapper.Map<IEnumerable<ProductDto>>(products);

            return productsDtos;
        }
    }
}
