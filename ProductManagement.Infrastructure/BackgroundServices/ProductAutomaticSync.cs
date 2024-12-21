using Microsoft.Extensions.Hosting;

namespace Random.App.ProductManagement.Infrastructure.BackgroundServices
{
    public class ProductAutomaticSync : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

