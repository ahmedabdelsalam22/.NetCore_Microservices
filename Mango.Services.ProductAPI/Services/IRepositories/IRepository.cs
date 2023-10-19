using System.Linq.Expressions;

namespace Mango.Services.ProductAPI.Services.IRepositories
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAll(bool tracked = true);
        Task<T> Get(Expression<Func<T, bool>>? filter = null, bool tracked = true);
        Task Create(T entity);
        Task Delete(T entity);
    }
}
