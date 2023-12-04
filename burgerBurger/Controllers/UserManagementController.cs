using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using burgerBurger.Models;
using burgerBurger.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace burgerBurger.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class UserManagementController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;


        public UserManagementController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var users = (from user in _userManager.Users
                         join userRole in _context.UserRoles on user.Id equals userRole.UserId
                         join role in _context.Roles on userRole.RoleId equals role.Id
                         select new UserViewModel
                         {
                             Id = user.Id,
                             Email = user.Email,
                             CurrentRole = role.Name,
                             AvailableRoles = _roleManager.Roles.Select(r => r.Name).ToList()
                         }).ToList();

            return View(users);
        }

        [HttpPost]
        public IActionResult ChangeRole(string userId, string newRole)
        {
            var user = _userManager.FindByIdAsync(userId).Result;
            var currentRoles = _userManager.GetRolesAsync(user).Result;

            var removeResult = _userManager.RemoveFromRolesAsync(user, currentRoles).Result;
            if (!removeResult.Succeeded)
            {
                return View("Error");
            }

            var addResult = _userManager.AddToRoleAsync(user, newRole).Result;
            if (!addResult.Succeeded)
            {
                return View("Error");
            }

            return RedirectToAction("Index");
        }
    }
}
