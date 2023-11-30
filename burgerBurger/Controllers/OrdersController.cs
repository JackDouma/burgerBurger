using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using burgerBurger.Data;
using burgerBurger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace burgerBurger.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var u = _userManager.Users.Where(u => u.UserName == "michaelrosanelli105@gmail.com").First();
            await _userManager.RemoveFromRoleAsync(u, "Manager");
            var res = await _userManager.IsInRoleAsync(u, "Manager");
            await _userManager.AddToRoleAsync(u, "Manager");
            res = await _userManager.IsInRoleAsync(u, "Manager");
            //var userManager = scope.service
            // if user is not an admin, show only their orders
            if (User.IsInRole("Customer"))
                return View(await _context.Orders.Where(o => o.CustomerId == User.Identity.Name).OrderBy(o => o.OrderId).ToListAsync());
            else if (User.IsInRole("Manager")) {
                var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                Response.Headers.Add("Refresh", "5");
                return View(await _context.Orders.Where(o => o.LocationId == location).ToListAsync());
            }
            else
                return View(await _context.Orders.OrderByDescending(o => o.OrderId).ToListAsync());

        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Administrator") || User.IsInRole("Owner"))
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails).ThenInclude(od => od.Item)
                    .Include(i => i.Location)
                    .FirstOrDefaultAsync(m => m.OrderId == id);
                if (order == null)
                {
                    return NotFound();
                }

                ViewData["location"] = _context.Location.FirstOrDefaultAsync(l => l.LocationId == order.LocationId).Result.DisplayName;
                return View(order);
            }
            else if (User.IsInRole("Customer"))
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails).ThenInclude(od => od.Item)
                    .Where(o => o.CustomerId == User.Identity.Name)
                    .FirstOrDefaultAsync(m => m.OrderId == id);
                if (order == null)
                {
                    return RedirectToAction("Index");
                }

                ViewData["location"] = _context.Location.FirstOrDefaultAsync(l => l.LocationId == order.LocationId).Result.DisplayName;
                return View(order);
            }
            else if (User.IsInRole("Manager"))
            {
                var user = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result;
                var order = await _context.Orders
                    .Include(o => o.OrderDetails).ThenInclude(od => od.Item)
                    .Where(o => o.LocationId == user.locationIdentifier)
                    .FirstOrDefaultAsync(m => m.OrderId == id);
                if (order == null)
                {
                    return RedirectToAction("Index");
                }

                ViewData["location"] = _context.Location.FirstOrDefaultAsync(l => l.LocationId == order.LocationId).Result.DisplayName;
                return View(order);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Manager,Owner,Admin")]
        public async Task<IActionResult> Update(int ItemId, string Status)
        {
            var order = await _context.Orders.FindAsync(ItemId);
            if(order != null)
            {
                order.Status = Status;
                _context.Update(order);
                _context.SaveChanges();
                return RedirectToAction("Details", new { @id = order.OrderId });
            }
            return RedirectToAction("Index");

        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
