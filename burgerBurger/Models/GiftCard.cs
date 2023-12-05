using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class GiftCard
    {
        public int GiftCardId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [Display(Name = "Dollar Amount")]
        public decimal amount { get; set; }

        public string giftPhoneNumber { get; set; }


        public string? code { get; set; }

        [Display(Name = "Redeemed")]
        public Boolean redeemed { get; set; }

        [Display(Name = "Customer Email")]
        [MaxLength(100)]
        public string? CustomerId { get; set; }

        public GiftCard()
        {
            redeemed = false;
        }
    }
}
