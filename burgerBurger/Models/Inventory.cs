using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace burgerBurger.Models
{
    public class Inventory
    {
        // variables
        public int InventoryId { get; set; }

        [Required]
        [Display(Name = "Name")]
        [MaxLength(25)]
        public string? itemName { get; set; }

        [Required]
        [Display(Name = "Description")]
        [MaxLength(100)]
        public string? itemDescription { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        [Range(1, 10000, ErrorMessage = "Quantity must be between 1-10,000")]
        public int quantity { get; set; }

        [Required]
        [Display(Name = "Calories")]
        [Range(0, 2000, ErrorMessage = "Calories must be between 0-2000")]
        public int calories { get; set; }

        [Required]
        [Display(Name = "Item Cost")]
        [Column(TypeName = "decimal(10,2)")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal itemCost { get; set; }

        [Required]
        [Display(Name = "Shelf Life(days)")]
        [Range(0, 2000, ErrorMessage = "Shelf Life must be between 1-365")]
        public int itemShelfLife { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Delivery Date")]
        public DateTime itemDeliveryDate { get; set; }

        [Display(Name = "Expiry")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime itemExpirey { get; set; }

        [Display(Name = "Expire Check")]
        public Boolean itemExpireCheck { get; set; }

        [Display(Name = "Thrown Out")]
        public Boolean itemThrowOutCheck { get; set; }

        // forgien key
        [Display(Name = "Location")]
        public int LocationId { get; set; }

        // parent ref
        public Location? Location { get; set; }

        // set default values
        public Inventory()
        {
            quantity = 1;
            itemThrowOutCheck = false;
            itemDeliveryDate = DateTime.Now;
        }
    }
}
