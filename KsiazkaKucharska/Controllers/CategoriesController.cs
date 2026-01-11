using KsiazkaKucharska.Data;
using KsiazkaKucharska.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KsiazkaKucharska.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
            => View(await _db.Categories.OrderBy(x => x.Name).ToListAsync());

        public IActionResult Create() => View(new Category());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cat = await _db.Categories.FindAsync(id);
            if (cat == null) return NotFound();

            // If category has recipes, you can block delete or reassign; here we block.
            var hasRecipes = await _db.Recipes.AnyAsync(r => r.CategoryId == id);
            if (hasRecipes)
            {
                ModelState.AddModelError("", "Cannot delete category used by recipes.");
                return View(cat);
            }

            _db.Categories.Remove(cat);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
