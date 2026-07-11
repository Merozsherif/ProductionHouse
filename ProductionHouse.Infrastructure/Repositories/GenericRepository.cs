using Microsoft.EntityFrameworkCore;
using ProductionHouse.Core.Interfaces;
using ProductionHouse.Infrastructure.Data;
using System.Linq.Expressions;
        

namespace ProductionHouse.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
 
        }

        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);

        }

          
    public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
    
        }

        public virtual async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet
                .Where(predicate)
                .ToListAsync();
        }
    }
}