using Microsoft.AspNetCore.Identity;

namespace burgerBurger.Data
{
    public class ApplicationUser : IdentityUser
    {
        public decimal? balance { get; set; }
        public int? locationIdentifier {  get; set; }
        public string? address {  get; set; }
        public string? city { get; set; }
        public string? province { get; set; }
        public string? country { get; set; }
        public string? postalCode { get; set; }
    }
}
