using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using burgerBurger.Data;

namespace burgerBurger.Controllers
{
    public class CustomItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CustomItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CustomItems
        public async Task<IActionResult> Index()
        {
            return _context.CustomItem != null ?
                        View(await _context.CustomItem.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.CustomItem'  is null.");
        }

        // GET: CustomItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomItem == null)
            {
                return NotFound();
            }

            var customItem = await _context.CustomItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customItem == null)
            {
                return NotFound();
            }

            return View(customItem);
        }

        // GET: CustomItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CustomItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,totalCalories,Price,Photo")] CustomItem customItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customItem);
        }

        // GET: CustomItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomItem == null)
            {
                return NotFound();
            }

            var customItem = await _context.CustomItem.FindAsync(id);
            if (customItem == null)
            {
                return NotFound();
            }
            return View(customItem);
        }

        // POST: CustomItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,totalCalories,Price,Photo")] CustomItem customItem)
        {
            if (id != customItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomItemExists(customItem.Id))
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
            return View(customItem);
        }

        // GET: CustomItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomItem == null)
            {
                return NotFound();
            }

            var customItem = await _context.CustomItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customItem == null)
            {
                return NotFound();
            }

            return View(customItem);
        }

        // POST: CustomItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CustomItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.CustomItem'  is null.");
            }
            var customItem = await _context.CustomItem.FindAsync(id);
            if (customItem != null)
            {
                _context.CustomItem.Remove(customItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomItemExists(int id)
        {
            return (_context.CustomItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
