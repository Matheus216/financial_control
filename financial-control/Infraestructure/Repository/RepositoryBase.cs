using financial_control.Infraestructure.Context;
using Microsoft.EntityFrameworkCore;

namespace financial_control.Infraestructure.Repository;

public class RepositoryBase<T> where T : class
{
    private readonly DbFinancialContext _context;

    public RepositoryBase(DbFinancialContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T> GetById(long id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task Insert(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
    }   
}