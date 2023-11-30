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
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Locations
        public async Task<IActionResult> Index()
        {
              return _context.Location != null ? 
                          View(await _context.Location.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Location'  is null.");
        }

        // GET: Locations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

  

        // GET: Locations/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocationId,LocationName,locationCity,locationProvince,locationAddress")] Location location, string OpeningTime, string ClosingTime)
        {
            location.OpeningTime = TimeOnly.Parse(OpeningTime);
            location.ClosingTime = TimeOnly.Parse(ClosingTime);
            if (ModelState.IsValid)
            {
                // set the display name to the name of the location, followed by the full address
                location.DisplayName = location.LocationName + " - " + location.locationCity + " " + location.locationProvince + " " + location.locationAddress;
                _context.Add(location);
                await _context.SaveChangesAsync();            
                return RedirectToAction(nameof(Index));
            }
            return View(location);
        }

        // GET: Locations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }
            ViewData["Opening"] = (location.OpeningTime.Value.Hour < 10 ? "0" + location.OpeningTime.Value.Hour : location.OpeningTime.Value.Hour) + ":" +
                (location.OpeningTime.Value.Minute < 10 ? "0" + location.OpeningTime.Value.Minute : location.OpeningTime.Value.Minute) + ":00";
            ViewData["Closing"] = (location.ClosingTime.Value.Hour < 10 ? ("0" + location.ClosingTime.Value.Hour) : location.ClosingTime.Value.Hour) + ":" +
                (location.ClosingTime.Value.Minute < 10 ? ("0" + location.ClosingTime.Value.Minute) : location.ClosingTime.Value.Minute) + ":00";

            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LocationId,LocationName,locationCity,locationProvince,locationAddress")] Location location, string OpeningTime, string ClosingTime)
        {
            if (id != location.LocationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    location.OpeningTime = TimeOnly.Parse(OpeningTime);
                    location.ClosingTime = TimeOnly.Parse(ClosingTime);
                    location.DisplayName = location.LocationName + " - " + location.locationCity + " " + location.locationProvince + " " + location.locationAddress;
                    _context.Update(location);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationExists(location.LocationId))
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
            return View(location);
        }

        // GET: Locations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Location == null)
            {
                return NotFound();
            }

            var location = await _context.Location
                .FirstOrDefaultAsync(m => m.LocationId == id);
            if (location == null)
            {
                return NotFound();
            }

            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Location == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Location'  is null.");
            }
            var location = await _context.Location.FindAsync(id);
            if (location != null)
            {
                _context.Location.Remove(location);

                var inventories = _context.Inventory.Where(i => i.LocationId == id);

                foreach (var inventory in inventories)
                {
                    _context.Inventory.Remove(inventory);
                }
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationExists(int id)
        {
          return (_context.Location?.Any(e => e.LocationId == id)).GetValueOrDefault();
        }
    }
}
