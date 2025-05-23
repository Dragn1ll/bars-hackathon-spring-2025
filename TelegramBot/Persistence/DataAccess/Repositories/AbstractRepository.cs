using System.Linq.Expressions;
using Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DataAccess.Repositories;

public abstract class AbstractRepository<TEntity>(AppDbContext context) : IRepository<TEntity> 
    where TEntity : class
{
    public async Task<bool> AddAsync(TEntity entity)
    {
        try
        {
            await context.Set<TEntity>()
                .AddAsync(entity);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> UpdateAsync(TEntity entity)
    {
        context.Set<TEntity>()
            .Update(entity);
        
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> PatchAsync(Guid id, Action<TEntity> patch)
    {
        try
        {
            var dbSet = context.Set<TEntity>();
            var entity = await dbSet.FindAsync(id);
            if (entity == null)
                return false;

            patch(entity);
            dbSet.Update(entity);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
    {
        try
        {
            var dbSet = context.Set<TEntity>();
            var entity = await dbSet.FirstOrDefaultAsync(predicate);
            if (entity == null)
                return false;
        
            dbSet.Remove(entity);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<TEntity?>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await context.Set<TEntity>()
            .AsNoTracking()
            .Where(filter)
            .ToListAsync();
    }

    public async Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await context.Set<TEntity>()
            .AsNoTracking()
            .Where(filter)
            .FirstOrDefaultAsync();
    }
}