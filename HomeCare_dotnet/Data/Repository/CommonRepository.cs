using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace HomeCare_dotnet.Data.Repository;

public class CommonRepository<T> : ICommonRepository<T> where T : class
{
    private readonly HomecareContext _dbContext;
    private DbSet<T> _dbSet;

    public CommonRepository(HomecareContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<T> CreateAsync(T dbRecord)
    {
        _dbSet.Add(dbRecord);
        await _dbContext.SaveChangesAsync();
        return dbRecord;
    }

    public async Task<bool> DeleteAsync(T dbRecord)
    {
        _dbSet.Remove(dbRecord);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
    {
        if(useNoTracking)   return await _dbSet.AsNoTracking().FirstOrDefaultAsync(filter);
        else return await _dbSet.FirstOrDefaultAsync(filter);
    }

    public async Task<T> UpdateAsync(T dbRecord)
    {
        _dbContext.Update(dbRecord);
        await _dbContext.SaveChangesAsync();
        return dbRecord;
    }
}
