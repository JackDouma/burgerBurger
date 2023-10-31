using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class Location
    {
        // variables
        public int LocationId { get; set; }

        [Required]
        [Display(Name = "City")]
        [MaxLength(25)]
        public string? locationCity { get; set; }

        [Required]
        [Display(Name = "Province")]
        [MaxLength(25)]
        public string? locationProvince { get; set; }

        [Required]
        [Display(Name = "Address")]
        [MaxLength(50)]
        public string? locationAddress { get; set; }

        // child ref
        public List<Inventory>? Inventories { get; set; }
    }
}
