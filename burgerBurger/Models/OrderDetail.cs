using burgerBurger.Models;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ItemId { get; set; }

        //Parent references. This is a junction table to reconcile the many-to-many orders->products relationship
        public Order? Order { get; set; }

        public OrderItem? Item { get; set; }
    }
}
