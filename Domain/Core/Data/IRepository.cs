namespace Domain.Core.Data
{
    public interface IRepository<TEntity, TKey> : IDisposable 
        where TEntity : class 
        where TKey : struct
    {
        Task Add(TEntity obj);
        Task Update(TEntity obj);
        Task<TEntity> GetByIdAsync(TKey id);
        IQueryable<TEntity> GetAll();
        IUnitOfWork UnitOfWork { get; }
     }
}
