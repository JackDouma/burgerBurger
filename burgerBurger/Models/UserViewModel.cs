using Microsoft.AspNetCore.Mvc;

namespace burgerBurger.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string CurrentRole { get; set; }
        public List<string> AvailableRoles { get; set; }
    }
}