using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public abstract class OrderItem
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Name { get; set; }

        [Range(0, int.MaxValue)]
        [DisplayName("Total Calories")]
        public int totalCalories { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public double Price { get; set; }

        public List<InventoryOutline>? Ingredients { get; set; }

        public string? Photo { get; set; }

        [Range(0, double.MaxValue)]
        [DisplayName("Promotion")]
        public double discountPercentage { get; set; }  

        public OrderItem()
        {
            Ingredients = new List<InventoryOutline>();
        }
    }
}