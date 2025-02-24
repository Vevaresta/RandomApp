using RandomApp.ProductManagement.Domain.Models;
using RandomApp.ProductManagement.Domain.Enums;
using Microsoft.Extensions.Hosting;
using RandomApp.ProductManagement.Application.Orchestrators;
using Microsoft.Extensions.DependencyInjection;
using RandomApp.ProductManagement.Application.Services.Interfaces;

namespace RandomApp.Web.Client.Services
{
    public class ProductSyncService : BackgroundService, IProductSyncService
    {
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
            _scopeFactory = scopeFactory;
        }

        private async Task<SyncResult> InitiateSyncAsync(ProductSyncRequestType requestType = ProductSyncRequestType.MANUAL)
        {

            if (!await _synLock.WaitAsync(TimeSpan.Zero))
            {
                return new SyncResult { Message = "Sync already in progress" };
            }

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var orchestrator = scope.ServiceProvider.GetRequiredService<IProductSyncOrchestrator>();

                _currentSyncStatus = new ProductSyncStatus
                {
                    IsSyncRunning = true,
                    LastRequestType = requestType,
                    LastSyncTime = DateTime.Now
                };

                OnSyncStatusChanged?.Invoke(_currentSyncStatus);

                var startTime = DateTime.Now;

                var result = await orchestrator.SyncProducts();

                if (result.Success)
                {
                    _currentSyncStatus.LastResultType = ProductSyncResultType.SUCCESS;
                }
                else
                {
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
            }

        }

        public async Task<SyncResult> InitiateSyncAsync()
        {
            return await InitiateSyncAsync(ProductSyncRequestType.MANUAL);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            await InitiateSyncAsync(ProductSyncRequestType.AUTOMATIC);

            using PeriodicTimer timer = new(TimeSpan.FromHours(24));

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                await InitiateSyncAsync(ProductSyncRequestType.AUTOMATIC);
            }

        }
    }
}
