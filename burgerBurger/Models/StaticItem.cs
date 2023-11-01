using burgerBurger.Enums;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class StaticItem
    {

        public int StaticItemId { get; set; }

        [Required]
        public StaticItemType Type { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public double Price { get; set; }

        public List<Inventory>? Ingredients { get; set; }

        public StaticItem() { 
            Ingredients = new List<Inventory>();
        }

    }
}
