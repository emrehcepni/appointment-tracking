using System.Linq.Expressions;
using AppointmentTracking.Domain.Entities;

namespace AppointmentTracking.Infrastructure.Repositories.Interfaces;

public interface IGenericRepository<TEntity, TPrimaryKey>
    where TPrimaryKey : struct
    where TEntity : Entity<TPrimaryKey>
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities);
    Task RemoveAsync(TEntity entity);
    Task RemoveByIdAsync(TPrimaryKey id);
    Task DeleteAsync(TEntity entity);
    Task DeleteByIdAsync(TPrimaryKey id, Guid userId);
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
    TEntity? FirstOrDefaultWithAsNoTracking(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> FirstOrDefaultWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetAllWithAsNoTrackingAsync();
    Task<TEntity?> GetById(TPrimaryKey id);
    Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
    IEnumerable<TEntity> WhereWithAsNoTracking(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> WhereWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate);
    bool BoolResult(int value);
}
