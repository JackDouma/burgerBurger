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
using Stripe.Checkout;
using Stripe;

namespace burgerBurger.Controllers
{
    [Authorize(Roles = "Admin,Customer")]
    public class BalanceAdditionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IConfiguration _configuration;

        public BalanceAdditionsController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: BalanceAdditions
        public async Task<IActionResult> Index()
        {
            ViewData["funds"] = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.balance;
            if (User.IsInRole("Admin"))
                return _context.BalanceAdditions != null ? 
                    View(await _context.BalanceAdditions.OrderByDescending(b => b.PaymentDate).ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.BalanceAdditions'  is null.");
            else
                return _context.BalanceAdditions != null ?
                    View(await _context.BalanceAdditions.Where(b => b.CustomerId == User.Identity.Name).ToListAsync()) :
                    Problem("Entity set 'ApplicationDbContext.BalanceAdditions'  is null.");
        }

        // GET: BalanceAdditions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BalanceAdditions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BalanceAdditionId,Amount,CustomerId,PaymentCode")] BalanceAddition balance)
        {
            balance.CustomerId = User.Identity.Name;
            HttpContext.Session.SetObject("Balance", balance);
            return RedirectToAction("Payment");
        }

        [Authorize(Roles = "Customer,Admin")]
        public IActionResult Payment()
        {
            // get the order from the session var
            var balance = HttpContext.Session.GetObject<BalanceAddition>("Balance");

            Session session = PaymentMethods.Payment(_configuration, balance.Amount, Request.Host.Value, "/BalanceAdditions/SaveOrder", "/Home/Account");

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        [Authorize]
        public IActionResult SaveOrder()
        {
            // get the order from session var
            var balance = HttpContext.Session.GetObject<BalanceAddition>("Balance");
            balance.PaymentCode = GetCustomerId();
            balance.PaymentDate = DateTime.Now;

            _context.BalanceAdditions.Add(balance);
            _context.SaveChanges();

            var user = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result;
            user.balance += balance.Amount;
            _context.Users.Update(user);

            balance.Balance = user.balance;
            _context.BalanceAdditions.Update(balance);
            _context.SaveChanges();

            // clear session variables
            HttpContext.Session.Clear();

            // redirect to Order Confirmation
            return RedirectToAction("Index");
        }

        private string GetCustomerId()
        {
            // check if we already have a session var for CustomerId
            if (HttpContext.Session.GetString("CustomerId") == null)
            {
                // create new session var of type string using a Guid
                HttpContext.Session.SetString("CustomerId", Guid.NewGuid().ToString());
            }

            return HttpContext.Session.GetString("CustomerId");
        }

        private bool BalanceAdditionExists(int id)
        {
          return (_context.BalanceAdditions?.Any(e => e.BalanceAdditionId == id)).GetValueOrDefault();
        }
    }
}
