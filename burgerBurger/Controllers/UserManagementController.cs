using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using burgerBurger.Models;
using burgerBurger.Data;

namespace burgerBurger.Controllers
{
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public UserManagementController(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var users = (from user in _userManager.Users
                         join userRole in _context.UserRoles on user.Id equals userRole.UserId
                         join role in _context.Roles on userRole.RoleId equals role.Id
                         select new UserViewModel
                         {
                             Email = user.Email,
                             Role = role.Name
                         }).ToList();

            return View(users);
        }
    }
}
