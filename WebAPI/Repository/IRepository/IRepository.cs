using System.Linq.Expressions;

namespace WebAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task Create(T entity);
        Task<List<T>> GetAll(Expression<Func<T, bool>> filtro = null);
        Task<T> Get(Expression<Func<T, bool>> filtro = null, bool tracked=true);
        Task Remove(T entity);
        Task Save();
    }
}
