using RandomApp.ProductManagement.Domain.Models;

namespace RandomApp.ProductManagement.Application.Services.Interfaces
{
    public interface IProductSyncService
    {
        public Task<SyncResult> InitiateSyncAsync();
        public ProductSyncStatus? CurrentSyncStatus { get; }

        public event Action<ProductSyncStatus> OnSyncStatusChanged;
    }
}
