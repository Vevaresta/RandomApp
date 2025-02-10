
using Common.Shared.Repositories;
using RandomApp.ShoppingCartManagement.Infrastructure.DataAccess;

namespace RandomApp.ShoppingCartManagement.Infrastructure.RepositoryImplementation
{
    public class ShoppingCartUnitOfWork : IUnitOfWork
    {
        private readonly ShoppingCartDbContext _context;
        private bool _disposed;

        public ShoppingCartUnitOfWork(ShoppingCartDbContext context)
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

