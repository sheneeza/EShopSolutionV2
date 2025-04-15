using Microsoft.EntityFrameworkCore;
using ReviewsAPI.ApplicationCore.Contracts.Repositories;
using ReviewsAPI.Infrastructure.Data;

namespace ReviewsAPI.Infrastructure.Repositories;

public class BaseRepository<T>: IRepository<T> where T : class
{
    protected readonly ReviewsDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(ReviewsDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }


    public async Task<int> InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsync(T entity)
    {
        _dbSet.Entry(entity).State = EntityState.Modified;
        return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsync(int id)
    {
        var item = await _dbSet.FindAsync(id);
        if (item != null)
        {
            _dbSet.Remove(item);
            return await _context.SaveChangesAsync();
        }
        return 0;
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }
}