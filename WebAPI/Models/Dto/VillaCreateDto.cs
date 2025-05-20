using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Dto
{
    public class VillaCreateDto
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public string Detail { get; set; }

        [Required]
        public double Price { get; set; }

        public int Occupants { get; set; }

        public double SquareMeters { get; set; }

        public string ImageUrl { get; set; }

        public string RegisterUser { get; set; }

        public DateTime RegisterDate { get; set; }

    }
}
