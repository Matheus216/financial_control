using financial_control_domain.Interfaces.Repositories;
using financial_control.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using financial_control_Domain.Entities;

namespace financial_control_Infrastructure.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly DbFinancialContext _context;

    public RepositoryBase(DbFinancialContext context)
    {
        _dbSet = context.Set<T>();
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAllAsync(
        int page = 1,
        int itemsPeerPage = 10,
        CancellationToken cancellationToken = default
    )
    {
        return await _dbSet
            .AsNoTracking()
            .Skip((page - 1) * itemsPeerPage)
            .Take(itemsPeerPage)
            .ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var response = await _dbSet.FindAsync(id, cancellationToken);
        return response;
    }

    public async Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync(cancellationToken);
    }   
}