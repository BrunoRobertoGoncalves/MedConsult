using Domain.Core.Data;
using Domain.Core.Models;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : Entity<TKey>, IAggregateRoot
        where TKey : struct
    {
        protected readonly ApplicationDataContext _dataContext;
        protected readonly DbSet<TEntity> _entity;
        protected readonly IUnitOfWork _unitOfWork;

        public Repository(ApplicationDataContext context)
        {
            _dataContext = context ?? throw new ArgumentNullException(nameof(context));
            _entity = _dataContext.Set<TEntity>();
        }

        public IUnitOfWork UnitOfWork => _dataContext;

        public virtual async Task Add(TEntity obj)
        {
            _entity.Add(obj);
        }
        public virtual Task Update(TEntity obj)
        {
            _entity.Update(obj);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _dataContext.Dispose();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _entity;
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            var item = await _entity.FindAsync(id);
            return item;
        }

    }
}
