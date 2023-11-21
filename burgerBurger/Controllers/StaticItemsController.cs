using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using burgerBurger.Data;
using burgerBurger.Models;

namespace burgerBurger.Controllers
{
    public class StaticItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StaticItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: StaticItems
        public async Task<IActionResult> Index()
        {
            return _context.StaticItem != null ?
                        View(await _context.StaticItem.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.StaticItem'  is null.");
        }

        // GET: StaticItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.StaticItem == null)
            {
                return NotFound();
            }

            var staticItem = await _context.StaticItem
                .FirstOrDefaultAsync(m => m.Id == id);
            var itemInventories = await _context.ItemInventory.Where(i => i.ItemId == id).ToListAsync();
            var inventories = await _context.InventoryOutline.ToListAsync();
            List<string> ings = new List<string>();

            // add ingredients to the staticItem object to be displayed
            foreach (var item in itemInventories)
                staticItem.Ingredients.Add(inventories.First(i => i.InventoryOutlineId == item.IngredientId));

            if (staticItem == null)
            {
                return NotFound();
            }

            return View(staticItem);
        }

        // GET: StaticItems/Create
        public IActionResult Create()
        {
            //var inventory = _context.InventoryOutline.OrderBy(i => i.Category).ToList();
            ViewData["Ingredients"] = _context.InventoryOutline.OrderBy(i => i.Category).ToList();
            return View();
        }

        // POST: StaticItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Name,Description,Price")] StaticItem staticItem, string? Ingredients, IFormFile? Photo)
        {
            staticItem.IsSelling = true;

            // add photo field to object
            if (Photo != null)
                staticItem.Photo = UploadPhoto(Photo);

            if (ModelState.IsValid)
            {
                _context.Add(staticItem);
                await _context.SaveChangesAsync();
                foreach (string i in Ingredients.Split(" "))
                {
                    if (!string.IsNullOrEmpty(i))
                    {
                        int ing = int.Parse(i);
                        // increment the total calories of the item by the calories value of each selected ingredient
                        staticItem.totalCalories += _context.InventoryOutline.Find(ing).calories;
                        // add a record of the relationship between the newly made item and the ingredient
                        _context.ItemInventory.Add(new ItemInventory(staticItem.Id, ing));
                    }
                    
                }
                await _context.SaveChangesAsync();
                _context.StaticItem.Update(staticItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staticItem);
        }

        // GET: StaticItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.StaticItem == null)
            {
                return NotFound();
            }

            var staticItem = await _context.StaticItem.FindAsync(id);
            if (staticItem == null)
            {
                return NotFound();
            }
            //ViewData["Ingredients"] = new MultiSelectList(_context.InventoryOutline, "InventoryOutlineId", "itemName");
            ViewData["Ingredients"] = _context.InventoryOutline.OrderBy(i => i.Category).ToList();
            ViewData["ItemIngredients"] = _context.ItemInventory.Where(i => i.ItemId == id).ToList();
            return View(staticItem);
        }

        // POST: StaticItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Name,Description,Price")] StaticItem staticItem, string Ingredients, IFormFile? Photo, string? CurrentPhoto)
        {
            if (id != staticItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // update photo if it was changed
                    if (Photo != null)
                        staticItem.Photo = UploadPhoto(Photo);
                    else
                        staticItem.Photo = CurrentPhoto;

                    _context.Update(staticItem);
                    await _context.SaveChangesAsync();

                    // Remove all records in the ItemInventory that reference the deleted static item
                    var cor = _context.ItemInventory.Where(x => x.ItemId == staticItem.Id);
                    foreach (var item in cor)
                        _context.ItemInventory.Remove(item);
                    await _context.SaveChangesAsync();
                    // do the same process as in the create method
                    foreach (string i in Ingredients.Split(" "))
                    {
                        if (!string.IsNullOrEmpty(i))
                        {
                            int ing = int.Parse(i);
                            // increment the total calories of the item by the calories value of each selected ingredient
                            staticItem.totalCalories += _context.InventoryOutline.Find(ing).calories;
                            // add a record of the relationship between the newly made item and the ingredient
                            _context.ItemInventory.Add(new ItemInventory(id, ing));
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StaticItemExists(staticItem.Id))
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
            return View(staticItem);
        }


        // GET: StaticItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.StaticItem == null)
            {
                return NotFound();
            }

            var staticItem = await _context.StaticItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (staticItem == null)
            {
                return NotFound();
            }

            return View(staticItem);
        }

        // POST: StaticItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.StaticItem == null)
            {
                return Problem("Entity set 'ApplicationDbContext.StaticItem'  is null.");
            }
            var staticItem = await _context.StaticItem.FindAsync(id);
            if (staticItem != null)
            {
                _context.StaticItem.Remove(staticItem);
                // Remove all records in the ItemInventory that reference the deleted static item
                var cor = _context.ItemInventory.Where(x => x.ItemId == staticItem.Id);
                foreach (var item in cor)
                    _context.ItemInventory.Remove(item);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StaticItemExists(int id)
        {
            return (_context.StaticItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static string UploadPhoto(IFormFile Photo)
        {
            // get temporary location of uploaded file
            var filePath = Path.GetTempFileName();

            // create unique name to prevent overwrites
            // ie. photo.jpg => g6rjft7-photo.jpg
            var fileName = Guid.NewGuid() + "-" + Photo.FileName;

            // set destination path to wwwroot/img/staticItems
            var uploadPath = System.IO.Directory.GetCurrentDirectory() + "\\wwwroot\\img\\staticItems\\" + fileName;

            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                Photo.CopyTo(stream);
            }

            return fileName;
        }
    }
}