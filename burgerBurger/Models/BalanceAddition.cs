using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class BalanceAddition
    {
        public int BalanceAdditionId { get; set; }

        [Required]

        public decimal Amount { get; set; }

        public string? CustomerId { get; set; }

        public string? PaymentCode { get; set; }
        [Display(Name = "Balance After Purchase")]
        public decimal? Balance { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
