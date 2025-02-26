using AutoMapper;
using Common.Shared.Repositories;
using NLog;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Application.Services.Interfaces;
using RandomApp.ProductManagement.Domain.Entities;

namespace RandomApp.ProductManagement.Infrastructure.Services
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
            _logger.Info("Adding product {product} to database", productDto);
            var entity = _mapper.Map<Product>(productDto);
            await _productRepository.AddAsync(entity);
            await _unitOfWork.CompleteAsync();

            _logger.Info("Product successfully saved to database", productDto);
            return _mapper.Map<ProductDto>(entity);
        }

        public async Task<IEnumerable<ProductDto>> AddRangeAsync(IEnumerable<ProductDto> productDtos)
        {
            var productList = productDtos.ToList();
            var count = productList.Count;

            _logger.Info("Adding {count} products to database", count);

            var entities = _mapper.Map<IEnumerable<Product>>(productDtos);
            await _productRepository.AddRangeAsync(entities);
            await _unitOfWork.CompleteAsync();

            _logger.Info("{count} products successfully added to database", count);
            return _mapper.Map<IEnumerable<ProductDto>>(entities);
        }

        public async Task<IEnumerable<ProductDto>> FindByNameOrDescription(string searchTerm)
        {
            _logger.Info("Looking for product with search term {searchTerm}", searchTerm);
            var products = await _productRepository.Find(p =>
                p.Name.Contains(searchTerm) ||
                p.ProductDescription.Value.Contains(searchTerm));

            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productsDto;
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            _logger.Info("Fetching all products");
            var products = await _productRepository.GetAllAsync();
            var productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);
            return productsDto;
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            _logger.Info("Fetching product with an ID {id}", productId);
            var product = await _productRepository.GetByIdAsync(productId);
            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task<bool> Remove(int productId)
        {
            if (productId <= 0)
            {
                _logger.Warn("Product with ID {id} not found", productId);
                return false;
            }
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
            {
                _logger.Warn("The product with an ID {id} doesn't exist", productId);
                return false;
            }

            _productRepository.Remove(product);
            await _unitOfWork.CompleteAsync();
            _logger.Info("Product with an Id {id] successfully removed", productId);
            return true;
        }

        public async Task<bool> RemoveRange(IEnumerable<ProductDto> productDtos)
        {
            if (productDtos == null || !productDtos.Any())
            {
                _logger.Warn("No products specified for removal");
                return false;
            }

            var idsToRemove = productDtos.Select(p => p.Id).ToList();
            var productsToRemove = await _productRepository.Find(p => idsToRemove.Contains(p.Id));

            var count = productsToRemove.Count();
            if (count == 0)
            {
                _logger.Warn("None of the specified products were found in the database");
                return false;
            }

            _productRepository.RemoveRange(productsToRemove);
            await _unitOfWork.CompleteAsync();
            _logger.Info("Successfully removed {count} products", count);

            return true;
        }
    }
}
