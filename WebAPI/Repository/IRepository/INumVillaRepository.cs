using WebAPI.Models;

namespace WebAPI.Repository.IRepository
{
    public interface INumVillaRepository : IRepository<NumVilla>
    {
        Task<NumVilla> Update(NumVilla entity);
    }
}
