using financial_control.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace financial_control_Infrastructure.Repositories;

public class RepositoryBase<T> where T : class
{
    private readonly DbSet<T> _dbSet;
    private readonly DbFinancialContext _context;

    public RepositoryBase(DbFinancialContext context)
    {
        _dbSet = context.Set<T>();
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetById(long id)
    {
        var response = await _dbSet.FindAsync(id);
        return response;
    }

    public async Task<int> Insert(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> Update(T entity)
    {
        _dbSet.Update(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> Delete(T entity)
    {
        _dbSet.Remove(entity);
        return await _context.SaveChangesAsync();
    }   
}