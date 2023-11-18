using burgerBurger.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class InventoryOutline
    {
        // variables
        public int InventoryOutlineId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(25)]
        public string? itemName { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MaxLength(100)]
        public string? itemDescription { get; set; }

        [Display(Name = "Calories")]
        [Range(0, 2000, ErrorMessage = "Calories must be between 0-2000")]
        public int calories { get; set; }

        [Required]
        [Display(Name = "Item Cost")]
        [Column(TypeName = "decimal(10,2)")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal itemCost { get; set; }

        [Display(Name = "Shelf Life(days)")]
        [Range(0, 2000, ErrorMessage = "Shelf Life must be between 1-365")]
        public int itemShelfLife { get; set; }

        [Required]
        public InventoryCategory Category { get; set; }
    }
}
