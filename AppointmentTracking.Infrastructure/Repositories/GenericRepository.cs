using System.Linq.Expressions;
using AppointmentTracking.Domain.Entities;
using AppointmentTracking.Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AppointmentTracking.Infrastructure.Repositories;

public class GenericRepository<TEntity, TPrimaryKey> : IGenericRepository<TEntity, TPrimaryKey>
        where TPrimaryKey : struct
        where TEntity : Entity<TPrimaryKey>, new()
{
    private readonly AppDbContext _dbContext;

    public GenericRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (typeof(TPrimaryKey) == typeof(Guid))
            entity.Id = (TPrimaryKey)(object)Guid.NewGuid();

        entity.CreatedDate = DateTime.UtcNow;
        entity.IsDeleted = false;

        _dbContext.Entry(entity).State = EntityState.Added;

        await _dbContext.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task RemoveAsync(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;

        _dbContext.Entry(entity).State = EntityState.Deleted;

        await _dbContext.SaveChangesAsync();

        await Task.CompletedTask;
    }

    public async Task RemoveByIdAsync(TPrimaryKey id)
    {
        var entity = await GetById(id);
        if (entity is null) return;

        entity.UpdatedDate = DateTime.UtcNow;

        _dbContext.Entry(entity).State = EntityState.Deleted;

        await _dbContext.SaveChangesAsync();

        await Task.CompletedTask;
    }

    public async Task DeleteAsync(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;

        _dbContext.Entry(entity).State = EntityState.Modified;

        await _dbContext.SaveChangesAsync();

        await Task.CompletedTask;
    }

    public async Task<bool> DeleteByIdAsync(TPrimaryKey id, Guid userId)
    {
        var entity = await GetById(id);
        if (entity is null) return false;

        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        entity.UpdatedId = userId;

        _dbContext.Entry(entity).State = EntityState.Modified;

        var result = await _dbContext.SaveChangesAsync();
        return BoolResult(result);
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(predicate);
    }

    public TEntity? FirstOrDefaultWithAsNoTracking(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefault(predicate);
    }

    public async Task<TEntity?> FirstOrDefaultWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
    }

    public async Task<bool> AnyWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().AnyAsync(predicate);
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetAllWithAsNoTrackingAsync()
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetById(TPrimaryKey id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        entity.UpdatedDate = DateTime.UtcNow;

        _dbContext.Entry(entity).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();

        return entity;
    }

    public async Task<IEnumerable<TEntity>> UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            entity.UpdatedDate = DateTime.UtcNow;
        }
        await _dbContext.SaveChangesAsync();

        return entities;
    }

    public async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().Where(predicate).ToListAsync();
    }

    public IEnumerable<TEntity> WhereWithAsNoTracking(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbContext.Set<TEntity>().Where(predicate).AsNoTracking().ToList();
    }

    public async Task<IEnumerable<TEntity>> WhereWithAsNoTrackingAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _dbContext.Set<TEntity>().Where(predicate).AsNoTracking().ToListAsync();
    }

    public bool BoolResult(int value)
    {
        return value > 0;
    }
}
