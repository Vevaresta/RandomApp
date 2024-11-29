namespace Common.Shared.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> CompleteAsync();
        int Complete();
    }
}
