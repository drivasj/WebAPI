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
        public DbSet<NumVilla> numVillas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Estandar",
                    Detail = "test detail",
                    ImageUrl = "",
                    Occupants = 5,
                    SquareMeters = 20,
                    Price = 80,
                    RegisterDate = DateTime.Now,
                    RegisterUser = "drivasj"

                },
                new Villa()
                {
                    Id = 2,
                    Name = "VIP",
                    Detail = "test detail VIP",
                    ImageUrl = "",
                    Occupants = 8,
                    SquareMeters = 70,
                    Price = 180,
                    RegisterDate = DateTime.Now,
                    RegisterUser = "drivasj"

                }

             );

            base.OnModelCreating(modelBuilder);
        }
    }
}
