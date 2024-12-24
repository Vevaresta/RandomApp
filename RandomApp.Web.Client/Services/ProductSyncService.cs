using RandomApp.ProductManagement.Domain.Models;
using RandomApp.ProductManagement.Domain.Enums;
using Microsoft.Extensions.Hosting;
using NLog;
using RandomApp.ProductManagement.Application.Orchestrators;
using RandomApp.ProductManagement.Application.Services;

namespace RandomApp.Web.Client.Services
{
    public class ProductSyncService : BackgroundService, IProductSyncService
    {
        private readonly ILogger _logger;
        private readonly IProductSyncOrchestrator _productSyncOrchestrator;
        private readonly SemaphoreSlim _synLock = new(1, 1);
        private ProductSyncStatus? _currentSyncStatus;
        public ProductSyncStatus? CurrentSyncStatus
        {
            get { return _currentSyncStatus; }
        }
        public event Action<ProductSyncStatus>? OnSyncStatusChanged;

        public ProductSyncService(IProductSyncOrchestrator productSyncOrchestrator)
        {

            _logger = LogManager.GetCurrentClassLogger();
            _productSyncOrchestrator = productSyncOrchestrator;

        }

        public async Task<SyncResult> InitiateSyncAsync()
        {
            if (!await _synLock.WaitAsync(TimeSpan.Zero))
            {
                return new SyncResult { Message = "Sync already in progress" };
            }

            try
            {
                UpdateStatus(true, ProductSyncRequestType.MANUAL);
                var startTime = DateTime.Now;

                var result = await _productSyncOrchestrator.SyncProducts();
                UpdateStatusAfterSync(startTime, result.Success);

                return result;
            }

            finally
            {
                UpdateStatus(false, _currentSyncStatus?.LastRequestType ?? ProductSyncRequestType.MANUAL);
                _synLock.Release();
            }

        }

        private void UpdateStatus(bool isRunning, ProductSyncRequestType requestType)
        {
            _currentSyncStatus = new ProductSyncStatus
            {
                IsSyncRunning = isRunning,
                LastRequestType = requestType
            };

            OnSyncStatusChanged?.Invoke(_currentSyncStatus);
        }


        private void UpdateStatusAfterSync(DateTime startTime, bool success)
        {
            if (_currentSyncStatus != null)
            {

                _currentSyncStatus.LastResultType = success
                    ? ProductSyncResultType.SUCCESS
                    : ProductSyncResultType.FAILED;

                _currentSyncStatus.LastSyncTime = DateTime.Now;
                _currentSyncStatus.SyncDuration = DateTime.Now - startTime;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await InitiateSyncAsync();

            using PeriodicTimer timer = new(TimeSpan.FromHours(24));

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                UpdateStatus(true, ProductSyncRequestType.AUTOMATIC);
                await InitiateSyncAsync();
            }
        }
    }
}
