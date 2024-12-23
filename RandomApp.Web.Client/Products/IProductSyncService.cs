using RandomApp.ProductManagement.Domain.Models;

namespace RandomApp.Web.Client.Products
{
    public interface IProductSyncService : IDisposable
    {
        public Task InitiateSyncAsync();
        public ProductSyncStatus? CurrentSyncStatus { get; }

        public event Action<ProductSyncStatus> OnSyncStatusChanged;


    }
}
