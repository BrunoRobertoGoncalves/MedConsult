namespace Domain.Core.Data
{
    public interface IRepository<TEntity, TKey> : IDisposable 
        where TEntity : class 
        where TKey : struct
    {
        void Add(TEntity obj);
        void Update(TEntity obj);
        TEntity GetById(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
        IQueryable<TEntity> GetAll();
    }
}
