using WebAPI.Dates;
using WebAPI.Models;
using WebAPI.Repository.IRepository;

namespace WebAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _context;

        public VillaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<Villa> Update(Villa entity)
        {
            entity.LastUpdate = DateTime.Now;
            _context.Villas.Update(entity);

            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
