using burgerBurger.Models;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal OrderTotal { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Address { get; set; }

        [Required]
        [MaxLength(50)]
        public string? City { get; set; }

        [Required]
        [MaxLength(2)]
        public string? Province { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Postal Code")]
        public string? PostalCode { get; set; }

        [Required]
        [MaxLength(15)]
        public string? Phone { get; set; }

        [Required]
        [Display(Name = "Email")]
        [MaxLength(100)]
        public string? CustomerId { get; set; }

        [MaxLength(100)]
        public string? PaymentCode { get; set; }

        // Child Reference
        public List<OrderDetail>? OrderDetails { get; set; }
    }
}
