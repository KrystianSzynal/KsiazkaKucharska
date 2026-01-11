using KsiazkaKucharska.Data;
using KsiazkaKucharska.Models;
using Microsoft.AspNetCore.Mvc;

namespace KsiazkaKucharska.Controllers
{
    public class RatingsController : Controller
    {
        private readonly AppDbContext _db;
        public RatingsController(AppDbContext db) => _db = db;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int recipeId, int value)
        {
            if (value < 1 || value > 5) value = 5;

            var exists = await _db.Recipes.FindAsync(recipeId);
            if (exists == null) return NotFound();

            _db.Ratings.Add(new Rating { RecipeId = recipeId, Value = value });
            await _db.SaveChangesAsync();

            return RedirectToAction("Details", "Recipes", new { id = recipeId });
        }
    }
}
