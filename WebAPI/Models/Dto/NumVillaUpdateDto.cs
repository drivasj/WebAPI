using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models.Dto
{
    public class NumVillaUpdateDto
    {
        [Required]
        public int VillaNo { get; set; }

        [Required]
        public int VillaId { get; set; }

        public string Special_Detail { get; set; }

    }
}
