using AutoMapper;
using Common.Shared.Repositories;
using NLog;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;

namespace RandomApp.ProductManagement.Application.Services.Implementations
{
    public class ProductDbService : IProductDbService
    {
        private readonly IProductRepository _productRepository;        
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        // TODO logging
        public ProductDbService(IProductRepository productRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = LogManager.GetCurrentClassLogger();
        }

        public async Task<ProductDto> AddAsync(ProductDto productDto)
        {
            var entity = _mapper.Map<Product>(productDto);
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<ProductDto>(entity);
        }

        public async Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<ProductDto> productDtos)
        {
            var entities = _mapper.Map<IEnumerable<Product>>(productDtos);
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CompleteAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        public async Task<IEnumerable<ProductDto>> FindByNameOrDescription(string searchTerm)
        {
            var products = await _productRepository.Find(p =>
                p.Name.Contains(searchTerm) ||
                p.ProductDescription.Value.Contains(searchTerm));

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productsDto;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var products = await _productRepository.GetAllAsync();
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
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
