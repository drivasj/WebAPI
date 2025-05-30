using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebAPI.Dates;
using WebAPI.Repository.IRepository;

namespace WebAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Repository(ApplicationDbContext context) 
        { 
            _context = context;
            this.dbSet = _context.Set<T>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Create(T entity)
        {
           await dbSet.AddAsync(entity);
           await Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await Save();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtro"></param>
        /// <param name="tracked"></param>
        /// <returns></returns>
        public async Task<T> Get(Expression<Func<T, bool>> filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;

            if (!tracked)
            {
                query = query.AsNoTracking();
            }

            if(filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filtro"></param>
        /// <returns></returns>
        public async Task<List<T>> GetAll(Expression<Func<T, bool>> filtro = null)
        {
            IQueryable<T> query = dbSet;

            if(filtro != null)
            {
                query = query.Where(filtro);
            }

            return await query.ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
