using Common.Shared.Repositories;
using NLog;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Application.DataTransferObjects;
using RandomApp.ProductManagement.Domain.Models;
using RandomApp.ProductManagement.Domain.ValueObjects;
using RandomApp.ProductManagement.Domain.Enums;
using RandomApp.ProductManagement.Application.Services.Interfaces;
namespace RandomApp.ProductManagement.Application.Orchestrators
{
    public class ProductSyncOrchestrator : IProductSyncOrchestrator
    {

        private readonly ILogger _logger;
        private readonly IProductService _productService;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductSyncOrchestrator(ILogger logger, IProductService productService, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _productService = productService;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<SyncResult> SyncProducts()
        {

            var productDtos = await _productService.GetProductsFromApiAsync();
            if (!productDtos.Any())
            {
                _logger.Warn("No products retrieved from API");
                return new SyncResult { Message = "No products retrieved from API" };
            }

            var existingProducts = await _productRepository.GetAllAsync();
            var result = await UpdateProductDatabase(productDtos, existingProducts);

            await _unitOfWork.CompleteAsync();

            return new SyncResult
            {
                Message = "Products processed successfully.",
                NewProductsAdded = result.newCount,
                ProductsUpdated = result.updateCount,
                Success = true
            };

        }

        private async Task<(int newCount, int updateCount)> UpdateProductDatabase(
            IEnumerable<ProductDto> productDtos,
            IEnumerable<Product> existingProducts)
        {
            int newProducts = 0;
            int updatedProducts = 0;

            foreach (var dto in productDtos)
            {
                var existingProduct = existingProducts.FirstOrDefault
                    (p => p.OriginalApiId == dto.OriginalApiId);

                if (existingProduct == null)
                {
                    await AddNewProduct(dto);
                    newProducts++;
                }
                else
                {
                    UpdateExistingProduct(existingProduct, dto);
                    updatedProducts++;
                }
            }

            return (newProducts, updatedProducts);
        }

        private async Task AddNewProduct(ProductDto dto)
        {
            var price = new Price(dto.Amount, dto.Currency);
            var description = new ProductDescription(dto.ProductDescription);
            var category = Enum.Parse<Category>(dto.Category);

            var product = Product.Create(
                dto.OriginalApiId,
                dto.Name,
                price,
                SKU.Create(dto.SKU),
                category,
                description,
                dto.Image
                );

            await _productRepository.AddAsync(product);
            _logger.Info("Adding new/restored product with OriginalApiId {0}", product.OriginalApiId);
        }

        private async Task UpdateExistingProduct(Product existingProduct, ProductDto dto)
        {
            var price = new Price(dto.Amount, dto.Currency);
            var description = new ProductDescription(dto.ProductDescription);
            var category = Enum.Parse<Category>(dto.Category);

            existingProduct.UpdateProduct(dto.Name, price, category, description, dto.Image);
            _productRepository.Update(existingProduct);
            _logger.Info("Updating existing product with OriginalApiId: {0}", existingProduct.OriginalApiId);
        }

    }
}
