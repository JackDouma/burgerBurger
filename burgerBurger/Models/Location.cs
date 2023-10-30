using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class Location
    {

        public int LocationId { get; set; }

        [Required]
        [MaxLength(255)]
        public string? City { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Province { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Address { get; set; }

    }
}
