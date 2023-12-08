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
