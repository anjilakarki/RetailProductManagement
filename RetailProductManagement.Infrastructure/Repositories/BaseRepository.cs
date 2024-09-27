using Microsoft.EntityFrameworkCore;
using RetailProductManagement.Core.Contracts.Repositories;
using RetailProductManagement.Infrastructure.Data;

namespace RetailProductManagement.Core.Contracts.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T: class
{
    protected readonly RetailDbContext _dbContext;

    public BaseRepository(RetailDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbContext.Set<T>().ToListAsync();
    }
    

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbContext.Set<T>().FindAsync(id);
    }

    public async Task<T> AddAsync(T entity)
    {
        _dbContext.Set<T>().Add(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<int> UpdateAsync(T entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        return await _dbContext.SaveChangesAsync();
       
    }

    public async Task<int> DeleteAsync(int id)
    {
        var entity = await _dbContext.Set<T>().FindAsync(id);
        if (entity != null)
        {
            _dbContext.Set<T>().Remove(entity);
            return await _dbContext.SaveChangesAsync();
            
        }
        return 0;
    }
}