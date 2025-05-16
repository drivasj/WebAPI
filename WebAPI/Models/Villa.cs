using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI.Models
{
    public class Villa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }

        public string Detail { get; set; }

        [Required]
        public double Price {  get; set; }

        public int Occupants { get; set; }

        public double SquareMeters {  get; set; }

        public string ImageUrl { get; set; }

        public string RegisterUser { get; set; }

        public DateTime RegisterDate { get; set; }

        public string LastUpdateUser {  get; set; }

        public DateTime LastUpdate { get; set; }

    }
}
