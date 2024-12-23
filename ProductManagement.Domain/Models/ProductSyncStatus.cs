using RandomApp.ProductManagement.Domain.Enums;

namespace RandomApp.ProductManagement.Domain.Models
{
    public class ProductSyncStatus
    {
        public bool IsSyncRunning { get; set; }
        public ProductSyncRequestType? LastRequestType { get; set; }
        public ProductSyncResultType? LastResultType { get; set; }
        public TimeSpan? SyncDuration { get; set; }
    }
}
