using RandomApp.ProductManagement.Domain.Models;

namespace RandomApp.ProductManagement.Application.Orchestrators
{
    public interface IProductSyncOrchestrator
    {
        Task<SyncResult> SyncProducts();
    }
}
