using Common.Shared.Repositories;
using RandomApp.ProductManagement.Domain.Models;
using RandomApp.ProductManagement.Domain.RepositoryInterfaces;
using NLog;
using RandomApp.ProductManagement.Domain.Entities;
using RandomApp.ProductManagement.Application.Services;

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
            var products = await _productService.GetProductsFromApiAsync();
            if (!products.Any())
            {
                _logger.Warn("No products retrieved from API");
                return new SyncResult { Message = "No products retrieved from API" };
            }

            var existingProducts = await _productRepository.GetAllAsync();
            var (newCount, updateCount) = await UpdateProductDatabase(products, existingProducts);

            await _unitOfWork.CompleteAsync();

            return new SyncResult
            {
                Message = "Products processed successfully.",
                NewProductsAdded = newCount,
                ProductsUpdated = updateCount,
                Success = true
            };

        }

        private async Task<(int newCount, int updateCount)> UpdateProductDatabase(
            IEnumerable<Product> products,
            IEnumerable<Product> existingProducts)
        {
            int newProducts = 0;
            int updatedProducts = 0;

            foreach (var product in products)
            {
                var existingProduct = existingProducts.FirstOrDefault(p =>
                    p.OriginalApiId == product.OriginalApiId);

                if (existingProduct == null)
                {
                    await AddNewProduct(product);
                    newProducts++;
                }
                else
                {
                    UpdateExistingProduct(existingProduct, product);
                    updatedProducts++;
                }
            }

            return (newProducts, updatedProducts);
        }

        private async Task AddNewProduct(Product product)
        {
            // resets ID to 0 for database auto-increment
            product.Id = 0;
            await _productRepository.AddAsync(product);
            _logger.Info("Adding new/restored product with OriginalApiId {0}", product.OriginalApiId);
        }

        private void UpdateExistingProduct(Product existingProduct, Product newProduct)
        {
            existingProduct.Name = newProduct.Name;
            existingProduct.Price = newProduct.Price;
            existingProduct.Category = newProduct.Category;
            existingProduct.Description = newProduct.Description;
            existingProduct.Image = newProduct.Image;

            _productRepository.Update(existingProduct);
            _logger.Info("Updating existing product with OriginalApiId: {0}", existingProduct.OriginalApiId);
        }
    }
}
