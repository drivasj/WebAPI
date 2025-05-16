using Microsoft.EntityFrameworkCore;
using WebAPI.Models;


namespace WebAPI.Dates
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
    }
}
