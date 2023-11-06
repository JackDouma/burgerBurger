using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal Price { get; set; }

        [Required]
        public string? CustomerId { get; set; }

        [Required]
        public int ItemId { get; set; }

        public OrderItem? Item { get; set; }
    }
}