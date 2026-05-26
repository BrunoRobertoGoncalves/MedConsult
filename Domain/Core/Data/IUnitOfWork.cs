namespace Domain.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
        Task<bool> SaveEntitiesAsync();
    }
}
