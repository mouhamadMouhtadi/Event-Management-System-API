using EMS.Core.Entities;
using EMS.Core.Repository.Interfaces;
using EMS.Core.Specifications;
using EMS.Repository;
using EMS.Repository.Data.Contexts;
using Microsoft.EntityFrameworkCore;

public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
    where TEntity : BaseEntity<TKey>
{
    private readonly AppDbContext _context;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _context.Set<TEntity>()
            .Where(e => !e.IsDeleted)
            .ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TKey id)
    {
        var entity = await _context.Set<TEntity>().FindAsync(id);
        return (entity != null && !entity.IsDeleted) ? entity : null;
    }
    public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> spec)
    {
        return await SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), spec).ToListAsync();
    }

    public async Task<TEntity> GetByIdWithSpecAsync(ISpecification<TEntity, TKey> spec)
    {
        return await  SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), spec).FirstOrDefaultAsync();
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _context.Set<TEntity>().AddRangeAsync(entities);
    }

    public void Delete(TEntity entity)
    {
        entity.IsDeleted = true;
        _context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            entity.IsDeleted = true;

        _context.Set<TEntity>().RemoveRange(entities);
    }

    public void Update(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
    }
    public async Task<int> GetCountAsync(ISpecification<TEntity, TKey> spec)
    {
        return await SpecificationEvaluator<TEntity, TKey>.GetQuery(_context.Set<TEntity>(), spec).CountAsync();
    }
}
