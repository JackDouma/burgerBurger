using burgerBurger.Enums;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class StaticItem : OrderItem
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

    }
}
