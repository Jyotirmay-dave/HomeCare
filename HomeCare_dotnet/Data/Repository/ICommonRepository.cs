using System;
using System.Linq.Expressions;

namespace HomeCare_dotnet.Data.Repository;

public interface ICommonRepository<T>
{
    Task<List<T>> GetAllAsync();
    Task<T> GetAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
    // Task<T> GetByNameAsync(Expression<Func<T, bool>> filter);
    Task<T> CreateAsync(T dbRecord);
    Task<T> UpdateAsync(T dbRecord);
    Task<bool> DeleteAsync(T dbRecord);
}
