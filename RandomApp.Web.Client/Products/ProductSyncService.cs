using RandomApp.ProductManagement.Domain.Models;
using RandomApp.ProductManagement.Domain.Enums;
using Microsoft.Extensions.Hosting;
using NLog;

namespace RandomApp.Web.Client.Products
{
    public class ProductSyncService : BackgroundService, IProductSyncService
    {
        private readonly ILogger _logger;
        private readonly IProductService _productService;
        private ProductSyncStatus? _currentSyncStatus;
        public ProductSyncStatus? CurrentSyncStatus 
        {
            get { return _currentSyncStatus; }
        }
        public event Action<ProductSyncStatus>? OnSyncStatusChanged;

        public ProductSyncService(ILogger logger, IProductService productService)
        {

            _logger = logger;
            _productService = productService;
      
        }

        public async Task InitiateSyncAsync()
        {
            _currentSyncStatus.IsSyncRunning = true;
            _currentSyncStatus.LastRequestType = ProductSyncRequestType.AUTOMATIC;

            OnSyncStatusChanged?.Invoke(_currentSyncStatus);

            var startTime = DateTime.Now;

            var products = await _productService.GetProductsFromApiAsync();
            if (products == null || !products.Any())
            {
                _currentSyncStatus.LastResultType = ProductSyncResultType.FAILED;
                _logger.Error("Product sync failed - No produccts received from API");
                return;
            }

            _currentSyncStatus.LastResultType = ProductSyncResultType.SUCCESS;
            _currentSyncStatus.SyncDuration = DateTime.Now - startTime;

            _logger.Info($"Successfully synced {products.Count()} products");

            _currentSyncStatus.IsSyncRunning = false;
            OnSyncStatusChanged?.Invoke(_currentSyncStatus);

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitiateSyncAsync();

            using PeriodicTimer timer = new(TimeSpan.FromHours(24));

            while(!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                await InitiateSyncAsync();
            }

            _logger.Info("Product sync service is stopping");
                  
        }
        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.Info("Product sync service is stopping");
            await base.StopAsync(stoppingToken);
        }

    }
}
