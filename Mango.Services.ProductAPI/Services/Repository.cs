using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Services.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Mango.Services.CouponAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public async Task Create(T entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();

        }

        public Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = _dbSet;
            if (filter != null) 
            {
                query = query.Where(filter);
            }
            if (!tracked) 
            {
                query = query.AsNoTracking();
            }
            return query.FirstOrDefaultAsync();
        }

        public Task<List<T>> GetAll(bool tracked = true)
        {
            IQueryable<T> query = _dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            return query.ToListAsync();
        }
    }
}
