namespace Common.Infrastructure.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
        int Complete();
    }
}
