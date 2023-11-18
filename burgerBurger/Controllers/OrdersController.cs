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
using burgerBurger.Data;

namespace burgerBurger.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            // if user is not an admin, show only their orders
            if (!User.IsInRole("Admin"))
                return View(await _context.Orders.Where(o => o.CustomerId == User.Identity.Name).OrderByDescending(o => o.OrderId).ToListAsync());
            // otherwise, show all orders
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

            if (User.IsInRole("Administrator"))
            {
                var order = await _context.Orders
                    .Include(o => o.OrderDetails).ThenInclude(od => od.Item)
                    .Include(i => i.Location)
                    .FirstOrDefaultAsync(m => m.OrderId == id);
                if (order == null)
                {
                    return NotFound();
                }

                ViewData["location"] = _context.Location.Where(l => l.LocationId == order.LocationId).First().DisplayName;
                return View(order);
            }
            else
            {
                var order = await _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Item).Where(o => o.CustomerId == User.Identity.Name)
                    .FirstOrDefaultAsync(m => m.OrderId == id);
                if (order == null)
                {
                    return RedirectToAction("Index");
                }

                ViewData["location"] = _context.Location.Where(l => l.LocationId == order.LocationId).First().DisplayName;
                return View(order);
            }
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
