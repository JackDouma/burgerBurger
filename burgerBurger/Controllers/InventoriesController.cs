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

namespace burgerBurger.Controllers
{
    public class InventoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoriesController(ApplicationDbContext context)
        {
            _context = context;
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

            ViewData["locationId"] = locationId;

            var inventory = _context.Inventory
                .Where(i => i.Location.LocationId == locationId)
                .OrderBy(i => i.itemExpirey)
                .ToList();

            return View(inventory);
        }
        //GET: Inventories/BalanceSheet
        public IActionResult BalanceSheet(int locationId)
        {
            if (locationId == 0)
            {
                return NotFound();
            }

            ViewData["locationId"] = locationId;

            var inventory = _context.Inventory
                .Where(i => i.Location.LocationId == locationId)
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
            if (inventory == null)
            {
                return NotFound();
            }

            return View(inventory);
        }

        // GET: Inventories/Create
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "locationAddress");
            return View();
        }

        // POST: Inventories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventoryId,itemName,itemDescription,quantity,calories,itemCost,itemShelfLife,itemDeliveryDate,LocationId")] Inventory inventory)
        {
            if (ModelState.IsValid)
            {
                // get expirary date
                inventory.itemExpirey = inventory.itemDeliveryDate.AddDays(inventory.itemShelfLife);

                // expire check
                if (DateTime.Now > inventory.itemExpirey)
                {
                    inventory.itemExpireCheck = false;
                }
                else if(DateTime.Now <= inventory.itemExpirey)
                {
                    inventory.itemExpireCheck = true;
                }

                _context.Add(inventory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
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
            ViewData["LocationId"] = new SelectList(_context.Location, "LocationId", "locationAddress", inventory.LocationId);
            return View(inventory);
        }

        // POST: Inventories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryId,itemName,itemDescription,quantity,calories,itemCost,itemShelfLife,itemDeliveryDate,LocationId")] Inventory inventory)
        {
            if (id != inventory.InventoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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
