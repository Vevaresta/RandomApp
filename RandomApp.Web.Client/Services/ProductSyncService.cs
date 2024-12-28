using RandomApp.ProductManagement.Domain.Models;
using RandomApp.ProductManagement.Domain.Enums;
using Microsoft.Extensions.Hosting;
using NLog;
using RandomApp.ProductManagement.Application.Orchestrators;
using RandomApp.ProductManagement.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace RandomApp.Web.Client.Services
{
    public class ProductSyncService : BackgroundService, IProductSyncService
    {
        private readonly ILogger _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly SemaphoreSlim _synLock = new(1, 1);
        private ProductSyncStatus? _currentSyncStatus;
        public ProductSyncStatus? CurrentSyncStatus
        {
            get { return _currentSyncStatus; }
        }
        public event Action<ProductSyncStatus>? OnSyncStatusChanged;

        public ProductSyncService(IServiceScopeFactory scopeFactory)
        {

            _logger = LogManager.GetCurrentClassLogger();
            _scopeFactory = scopeFactory;
        }

        private async Task<SyncResult> InitiateSyncAsync(ProductSyncRequestType requestType = ProductSyncRequestType.MANUAL)
        {
            _logger.Debug("Attempting to inititate {requestType} sync", requestType);

            if (!await _synLock.WaitAsync(TimeSpan.Zero))
            {
                _logger.Info("Sync attempt rejected - another sync already in progress");
                return new SyncResult { Message = "Sync already in progress" };
            }

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var orchestrator = scope.ServiceProvider.GetRequiredService<IProductSyncOrchestrator>();

                _logger.Info("Starting {requestType} product sync", requestType);
                _currentSyncStatus = new ProductSyncStatus
                {
                    IsSyncRunning = true,
                    LastRequestType = requestType,
                    LastSyncTime = DateTime.Now
                };

                OnSyncStatusChanged?.Invoke(_currentSyncStatus);

                var startTime = DateTime.Now;

                _logger.Debug("Calling orchestrator.SyncProducts()");
                var result = await orchestrator.SyncProducts();

                if (result.Success)
                {
                    _logger.Info("Product synchronized  successfully in {Duration}.", DateTime.Now - startTime);
                    _currentSyncStatus.LastResultType = ProductSyncResultType.SUCCESS;
                }
                else
                {
                    _logger.Warn("Product synchronization failed with message {Message}.", result.Message);
                    _currentSyncStatus.LastResultType = ProductSyncResultType.FAILED;
                }

                _currentSyncStatus.SyncDuration = DateTime.Now - startTime;
                _currentSyncStatus.IsSyncRunning = false;
                // if there are any subs, notify them-> later make some UI component that subscribes to it
                OnSyncStatusChanged?.Invoke(_currentSyncStatus);

                return result;
            }

            finally
            {
                _synLock.Release();
                _logger.Debug("Sync lock released.");
            }

        }

        public async Task<SyncResult> InitiateSyncAsync()
        {
            _logger.Info("Activated manual synchronization.");
            return await InitiateSyncAsync(ProductSyncRequestType.MANUAL);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Info("Background service started");

            await InitiateSyncAsync(ProductSyncRequestType.AUTOMATIC);

            using PeriodicTimer timer = new(TimeSpan.FromHours(24));

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.Info("24-hour timer triggered, initiating automatic sync");
                await InitiateSyncAsync(ProductSyncRequestType.AUTOMATIC);
            }

            _logger.Info("Background service stopped");
        }
    }
}
