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

namespace burgerBurger.Controllers
{
    [Authorize(Roles = "Admin,Owner")]
    public class InventoryOutlinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InventoryOutlinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: InventoryOutlines
        public async Task<IActionResult> Index()
        {
              return _context.InventoryOutline != null ? 
                          View(await _context.InventoryOutline.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.InventoryOutline'  is null.");
        }

        // GET: InventoryOutlines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.InventoryOutline == null)
            {
                return NotFound();
            }

            var inventoryOutline = await _context.InventoryOutline
                .FirstOrDefaultAsync(m => m.InventoryOutlineId == id);
            if (inventoryOutline == null)
            {
                return NotFound();
            }

            return View(inventoryOutline);
        }

        // GET: InventoryOutlines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: InventoryOutlines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InventoryOutlineId,itemName,itemDescription,calories,itemCost,itemShelfLife,Category")] InventoryOutline inventoryOutline)
        {
            if (ModelState.IsValid)
            {
                if (inventoryOutline.Category == Enums.InventoryCategory.Package)
                {
                    inventoryOutline.calories = 0;
                    inventoryOutline.itemShelfLife = 0;
                }
                _context.Add(inventoryOutline);
                await _context.SaveChangesAsync();        
                return RedirectToAction(nameof(Index));
            }
            return View(inventoryOutline);
        }

        // GET: InventoryOutlines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.InventoryOutline == null)
            {
                return NotFound();
            }

            var inventoryOutline = await _context.InventoryOutline.FindAsync(id);
            if (inventoryOutline == null)
            {
                return NotFound();
            }
            return View(inventoryOutline);
        }

        // POST: InventoryOutlines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InventoryOutlineId,itemName,itemDescription,calories,itemCost,itemShelfLife,Category")] InventoryOutline inventoryOutline)
        {
            if (id != inventoryOutline.InventoryOutlineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (inventoryOutline.Category == Enums.InventoryCategory.Package)
                    {
                        inventoryOutline.calories = 0;
                        inventoryOutline.itemShelfLife = 0;
                    }
                    _context.Update(inventoryOutline);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InventoryOutlineExists(inventoryOutline.InventoryOutlineId))
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
            return View(inventoryOutline);
        }

        // GET: InventoryOutlines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.InventoryOutline == null)
            {
                return NotFound();
            }

            var inventoryOutline = await _context.InventoryOutline
                .FirstOrDefaultAsync(m => m.InventoryOutlineId == id);
            if (inventoryOutline == null)
            {
                return NotFound();
            }

            return View(inventoryOutline);
        }

        // POST: InventoryOutlines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.InventoryOutline == null)
            {
                return Problem("Entity set 'ApplicationDbContext.InventoryOutline'  is null.");
            }
            var inventoryOutline = await _context.InventoryOutline.FindAsync(id);
            if (inventoryOutline != null)
            {
                _context.InventoryOutline.Remove(inventoryOutline);

                var inventories = _context.Inventory.Where(i => i.Outline == id);

                foreach (var inventory in inventories)
                {
                    _context.Inventory.Remove(inventory);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InventoryOutlineExists(int id)
        {
          return (_context.InventoryOutline?.Any(e => e.InventoryOutlineId == id)).GetValueOrDefault();
        }
    }
}
