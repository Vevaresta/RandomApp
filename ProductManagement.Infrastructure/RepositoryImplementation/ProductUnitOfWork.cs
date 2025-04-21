using Common.Shared.Repositories;
using NLog;
using RandomApp.ProductManagement.Infrastructure.Persistence;

namespace RandomApp.ProductManagement.Infrastructure.RepositoryImplementation
{
    public class ProductUnitOfWork : IUnitOfWork
    {
        private readonly ProductDbContext _context;
        private bool _disposed;


        public ProductUnitOfWork(ProductDbContext context)
        {
            _context = context;

        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
