using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class Location
    {
        // variables
        public int LocationId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(25)]
        public string? LocationName { get; set; }

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

        public string? DisplayName { get; set; }

        [Display(Name = "Opening Time")]
        public TimeOnly? OpeningTime { get; set; }

        [Display(Name = "Closing Time")]
        public TimeOnly? ClosingTime { get; set; }
    }
}
