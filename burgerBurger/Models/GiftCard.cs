using System.ComponentModel.DataAnnotations;

namespace burgerBurger.Models
{
    public class GiftCard
    {
        public int GiftCardId { get; set; }
        [Required]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal amount { get; set; }

        public string? code { get; set; }

        public Boolean redeemed { get; set; }

        public GiftCard()
        {
            redeemed = false;
        }
    }
}
