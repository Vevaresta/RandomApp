using RandomApp.ProductManagement.Domain.Models;
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
        private Timer? _timer = null;
        private int executionCount = 0;

        public ProductSyncService(ILogger logger, IProductService productService)
        {

            _logger = logger;
            _productService = productService;
        }

        public Task InitiateSyncAsync()
        {
            var initiate = _productService.GetProductsFromApiAsync();
            return Task.CompletedTask;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            using PeriodicTimer timer = new(TimeSpan.FromHours(24));

            while(await timer.WaitForNextTickAsync(stoppingToken))
            {
                await InitiateSyncAsync();
            }
    
                
        }
        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void DoWork(object? state)
        {
            var count = Interlocked.Increment(ref executionCount);
            _logger.Info("Timed Hosted Service is working. Count: {Count}", count);
        }
    }
}
