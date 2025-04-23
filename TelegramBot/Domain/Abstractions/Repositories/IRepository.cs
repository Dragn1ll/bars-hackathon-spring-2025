using System.Linq.Expressions;

namespace Domain.Abstractions.Repositories;

public interface IRepository<TEntity>
{
    Task<bool> AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> PatchAsync(int id, Action<TEntity> patch);
    Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity?>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter);
    Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter);
}