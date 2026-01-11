using KsiazkaKucharska.Data;
using KsiazkaKucharska.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KsiazkaKucharska.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly AppDbContext _db;
        public IngredientsController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index()
            => View(await _db.Ingredients.OrderBy(x => x.Name).ToListAsync());

        public IActionResult Create() => View(new Ingredient());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Ingredient model)
        {
            if (!ModelState.IsValid) return View(model);

            _db.Ingredients.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var ing = await _db.Ingredients.FindAsync(id);
            if (ing == null) return NotFound();
            return View(ing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Ingredient model)
        {
            if (id != model.Id) return BadRequest();
            if (!ModelState.IsValid) return View(model);

            _db.Entry(model).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var ing = await _db.Ingredients.FindAsync(id);
            if (ing == null) return NotFound();
            return View(ing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ing = await _db.Ingredients.FindAsync(id);
            if (ing == null) return NotFound();

            var used = await _db.RecipeIngredients.AnyAsync(ri => ri.IngredientId == id);
            if (used)
            {
                ModelState.AddModelError("", "Cannot delete ingredient used by recipes.");
                return View(ing);
            }

            _db.Ingredients.Remove(ing);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
