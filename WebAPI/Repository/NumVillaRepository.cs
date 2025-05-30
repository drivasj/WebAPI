using WebAPI.Dates;
using WebAPI.Models;
using WebAPI.Repository.IRepository;

namespace WebAPI.Repository
{
    public class NumVillaRepository : Repository<NumVilla>, INumVillaRepository
    {
        private readonly ApplicationDbContext _context;

        public NumVillaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<NumVilla> Update(NumVilla entity)
        {
            entity.LastUpdate = DateTime.Now;
            _context.numVillas.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
