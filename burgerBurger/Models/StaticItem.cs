/*using burgerBurger.Enums;
using System.ComponentModel;
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

        [Range(0, int.MaxValue)]
        [DisplayName("Total Calories")]
        public int totalCalories { get; set; }

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
*/

using burgerBurger.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class StaticItem : OrderItem
    {
        [Required]
        public StaticItemType Type { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Description { get; set; }

        public bool IsSelling { get; set; }
    }
}
