using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using burgerBurger.Data;
using burgerBurger.Models;
using System.Composition;
using Microsoft.AspNetCore.Authorization;

using System.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

using Twilio.TwiML;
using Twilio.AspNet.Mvc;

namespace burgerBurger.Controllers
{
    [Authorize(Roles = "Admin,Manager,Owner")]
    public class InventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public InventoriesController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        private void CheckAndSendTwilioMessages(Inventory inventory)
        {
            if (!inventory.itemExpireCheck && inventory.itemExpirey.Date <= DateTime.Now.AddDays(3).Date)
            {
                if (!inventory.MessageSent)
                {
                    TwilioClient.Init(_configuration.GetValue<string>("Twilio:AccountSID"), _configuration.GetValue<string>("Twilio:AuthToken"));

                    var locationId = inventory.LocationId;
                    var location = _context.Location.FirstOrDefault(l => l.LocationId == locationId);

   

                    var managerUserRole = _context.UserRoles
                        .FirstOrDefault(ur => ur.UserId == _context.Users
                                                            .Where(u => u.locationIdentifier == location.LocationId)
                                                            .Select(u => u.Id)
                                                            .FirstOrDefault() &&
                                                ur.RoleId == "2f3c1a4b-e860-4f72-9f35-026fdaff2dc8");

                    if (managerUserRole != null)
                    {
                        var managerUser = _context.Users.FirstOrDefault(u => u.Id == managerUserRole.UserId);

                        if (managerUser != null && !string.IsNullOrEmpty(managerUser.PhoneNumber))
                        {
                            var to = new PhoneNumber(managerUser.PhoneNumber);
                            var from = new PhoneNumber(_configuration.GetValue<string>("Twilio:PhoneNumber"));
                            var messageBody = $"Item '{inventory.itemName}' is about to expire on {inventory.itemExpirey = inventory.itemDeliveryDate.AddDays(inventory.itemShelfLife)}.";

                            var message = MessageResource.Create(
                                to: to,
                                from: from,
                                body: messageBody);

                            inventory.MessageSent = true;

                            _context.SaveChanges();
                        }
                    }
                }
                }
            }




        // GET: Inventories
        //public async Task<IActionResult> Index()
        //{ 
        //var applicationDbContext = _context.Inventory.Include(i => i.Location);
        //return View(await applicationDbContext.ToListAsync());
        //}

        //GET: Inventories
        public IActionResult Index(int locationId)
        {
            if (locationId == 0)
            {
                return NotFound();
            }

            if (User.IsInRole("Manager"))
            {
                var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                if (location != locationId)
                {
                    return RedirectToAction("Index", new { locationId = location });
                }
            }

            ViewData["locationId"] = locationId;
            
            var inventory = _context.Inventory
                .Where(i => i.Location.LocationId == locationId)
                .Where(i => i.itemThrowOutCheck == false)
                .OrderBy(i => i.itemExpirey)
                .ToList();

            return View(inventory);
        }

        [HttpGet]
        public IActionResult ThrowOut(int id)
        {
            var item = _context.Inventory.Find(id);

            if (item != null)
            {
                if (User.IsInRole("Manager"))
                {
                    var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                    if (location != item.LocationId)
                    {
                        return RedirectToAction("Index", new { locationId = location });
                    }
                }

                item.itemThrowOutCheck = true;
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { locationId = item.LocationId });
        }

        [HttpGet]
        public IActionResult UndoThrowOut(int id)
        {
            var item = _context.Inventory.Find(id);

            if (item != null)
            {
                if (User.IsInRole("Manager"))
                {
                    var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                    if (location != item.LocationId)
                    {
                        return RedirectToAction("Index", new { locationId = location });
                    }
                }

                item.itemThrowOutCheck = false;
                _context.SaveChanges();
            }

            return RedirectToAction("ThrownOutItems", new { locationId = item.LocationId });
        }

        //GET: Inventories/BalanceSheet
        public IActionResult BalanceSheet(int locationId)
        {
            if (locationId == 0)
            {
                return NotFound();
            }

            if (User.IsInRole("Manager"))
            {
                var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                if (location != locationId)
                {
                    return RedirectToAction("Index", new { locationId = location });
                }
            }

            var orders = _context.Orders
                .Where(o => o.LocationId == locationId)
                .ToList();

            ViewBag.Orders = orders;

            ViewData["locationId"] = locationId;

            var inventory = _context.Inventory
                .Where(i => i.Location.LocationId == locationId)
                .OrderBy(i => i.itemExpirey)
                .ToList();

            return View(inventory);
        }

        //GET: Inventories/BalanceSheet
        public IActionResult ThrownOutItems(int locationId)
        {
            if (locationId == 0)
            {
                return NotFound();
            }

            if (User.IsInRole("Manager"))
            {
                var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                if (location != locationId)
                {
                    return RedirectToAction("Index", new { locationId = location });
                }
            }

            ViewData["locationId"] = locationId;

            var inventory = _context.Inventory
                .Where(i => i.Location.LocationId == locationId)
                .Where(i => i.itemThrowOutCheck == true)
                .OrderBy(i => i.itemExpirey)
                .ToList();

            return View(inventory);
        }

        // GET: Inventories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .Include(i => i.Location)
                .FirstOrDefaultAsync(m => m.InventoryId == id);

            if (User.IsInRole("Manager"))
            {
                var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                if (location != inventory.LocationId)
                {
                    return RedirectToAction("Index", new { locationId = location });
                }
            }


            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            ViewData["Ingredients"] = new SelectList(_context.InventoryOutline, "InventoryOutlineId", "itemName");
            if (User.IsInRole("Manager"))
            {
                var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                ViewData["LocationId"] = new SelectList(_context.Location.Where(l => l.LocationId == location), "LocationId", "locationAddress");
            }
            else
            {
                ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "locationAddress");
            }
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Outline,InventoryId,quantity,itemDeliveryDate,LocationId")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Manager"))
                {
                    var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                    if (location != inventory.LocationId)
                    {
                        return RedirectToAction("Index", new { locationId = location });
                    }
                }

                InventoryOutline inventoryOutline = _context.InventoryOutline.Find(inventory.Outline);
                inventory.itemName = inventoryOutline.itemName;
                inventory.itemDescription = inventoryOutline.itemDescription;
                inventory.calories = inventoryOutline.calories;
                inventory.itemCost = inventoryOutline.itemCost;
                inventory.itemShelfLife = inventoryOutline.itemShelfLife;
                inventory.Category = inventoryOutline.Category;

                CheckAndSendTwilioMessages(inventory);

                // if package
                if (inventory.Category == Enums.InventoryCategory.Package)
                {
                    inventory.calories = 0;
                    inventory.itemShelfLife = 0;
                }
                // if not package
                else
                {
                    // get expirary date
                    inventory.itemExpirey = inventory.itemDeliveryDate.AddDays(inventory.itemShelfLife);

                    // expire check
                    if (DateTime.Now > inventory.itemExpirey)
                    {
                        inventory.itemExpireCheck = false;
                    }
                    else if (DateTime.Now <= inventory.itemExpirey)
                    {
                        inventory.itemExpireCheck = true;
                    }
                }                 

                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { locationId = inventory.LocationId });
            }
            ViewData["Ingredients"] = new SelectList(_context.InventoryOutline, "InventoryOutlineId", "itemName");
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "locationAddress", inventory.LocationId);
            return View(inventory);
        }

        // GET: Inventories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Manager"))
            {
                var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                if (location != inventory.LocationId)
                {
                    return RedirectToAction("Index", new { locationId = location });
                }
            }

            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "locationAddress", inventory.LocationId);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Outline,Category,InventoryId,itemName,itemDescription,quantity,calories,itemCost,itemShelfLife,itemDeliveryDate,LocationId")] Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("Manager"))
                    {
                        var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                        if (location != inventory.LocationId)
                        {
                            return RedirectToAction("Index", new { locationId = location });
                        }
                    }

                    InventoryOutline inventoryOutline = _context.InventoryOutline.Find(inventory.Outline);
                    inventory.itemName = inventoryOutline.itemName;
                    inventory.itemDescription = inventoryOutline.itemDescription;
                    inventory.calories = inventoryOutline.calories;
                    inventory.itemCost = inventoryOutline.itemCost;
                    inventory.itemShelfLife = inventoryOutline.itemShelfLife;
                    inventory.Category = inventoryOutline.Category;

                    CheckAndSendTwilioMessages(inventory);

                    // if package
                    if (inventory.Category == Enums.InventoryCategory.Package)
                    {
                        inventory.calories = 0;
                        inventory.itemShelfLife = 0;
                    }
                    // if not package
                    else
                    {
                        // get expirary date
                        inventory.itemExpirey = inventory.itemDeliveryDate.AddDays(inventory.itemShelfLife);

                        // expire check
                        if (DateTime.Now > inventory.itemExpirey)
                        {
                            inventory.itemExpireCheck = false;
                        }
                        else if (DateTime.Now <= inventory.itemExpirey)
                        {
                            inventory.itemExpireCheck = true;
                        }
                    }

                    _context.Update(inventory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryExists(inventory.InventoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "locationAddress", inventory.LocationId);
            return View(inventory);
        }

        // GET: Inventories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Inventory == null)
            {
                return NotFound();
            }

            var inventory = await _context.Inventory
                .Include(i => i.Location)
                .FirstOrDefaultAsync(m => m.InventoryId == id);
            if (inventory == null)
            {
                return NotFound();
            }

            if (User.IsInRole("Manager"))
            {
                var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                if (location != inventory.LocationId)
                {
                    return RedirectToAction("Index", new { locationId = location });
                }
            }

            return View(inventory);
        }

        // POST: Inventories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Inventory == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Inventory'  is null.");
            }
            var inventory = await _context.Inventory.FindAsync(id);

            if (inventory != null)
            {
                if (User.IsInRole("Manager"))
                {
                    var location = _context.Users.FirstOrDefaultAsync(u => u.UserName == User.Identity.Name).Result.locationIdentifier;
                    if (location != inventory.LocationId)
                    {
                        return RedirectToAction("Index", new { locationId = location });
                    }
                }
                _context.Inventory.Remove(inventory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryExists(int id)
        {
          return (_context.Inventory?.Any(e => e.InventoryId == id)).GetValueOrDefault();
        }
    }
}
