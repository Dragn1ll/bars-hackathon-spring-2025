using System.Linq.Expressions;

namespace Domain.Abstractions.Repositories;

public interface IRepository<in TId, TEntity>
{
    public Task<bool> AddAsync(TEntity entity);
    public Task<bool> UpdateAsync(TEntity entity);
    public Task<bool> PatchAsync(TId id, Action<TEntity> patch);
    public Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    public Task<IEnumerable<TEntity?>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter);
    public Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter);
}