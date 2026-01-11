using KsiazkaKucharska.Data;
using KsiazkaKucharska.Models;
using KsiazkaKucharska.VievModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KsiazkaKucharska.Controllers
{
    public class RecipesController : Controller
    {
        private readonly AppDbContext _db;
        public RecipesController(AppDbContext db) => _db = db;

        public async Task<IActionResult> Index(string? q, int? categoryId, List<int>? ingredientIds, bool matchAllIngredients = false)
        {
            ingredientIds ??= new List<int>();

            var query = _db.Recipes
                .AsNoTracking()
                .Include(r => r.Category)
                .Include(r => r.Ratings)
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
                query = query.Where(r => r.Title.Contains(q) || (r.Description ?? "").Contains(q));

            if (categoryId.HasValue)
                query = query.Where(r => r.CategoryId == categoryId.Value);

            if (ingredientIds.Count > 0)
            {
                if (matchAllIngredients)
                {
                    // Must contain ALL selected ingredients
                    query = query.Where(r =>
                        ingredientIds.All(id => r.RecipeIngredients.Any(ri => ri.IngredientId == id)));
                }
                else
                {
                    // ANY
                    query = query.Where(r =>
                        r.RecipeIngredients.Any(ri => ingredientIds.Contains(ri.IngredientId)));
                }
            }

            var results = await query
                .OrderBy(r => r.Title)
                .Select(r => new RecipeIndexRow
                {
                    Id = r.Id,
                    Title = r.Title,
                    CategoryName = r.Category != null ? r.Category.Name : "",
                    PrepTimeMinutes = r.PrepTimeMinutes,
                    AvgRating = r.Ratings.Count == 0 ? 0 : r.Ratings.Average(x => x.Value),
                    RatingsCount = r.Ratings.Count
                })
                .ToListAsync();

            var vm = new RecipeIndexVM
            {
                Q = q,
                CategoryId = categoryId,
                IngredientIds = ingredientIds,
                MatchAllIngredients = matchAllIngredients,
                Results = results,
                CategoryOptions = await _db.Categories.AsNoTracking()
                    .OrderBy(c => c.Name)
                    .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                    .ToListAsync(),
                IngredientOptions = await _db.Ingredients.AsNoTracking()
                    .OrderBy(i => i.Name)
                    .Select(i => new SelectListItem(i.Name, i.Id.ToString()))
                    .ToListAsync()
            };

            vm.CategoryOptions.Insert(0, new SelectListItem("(All categories)", ""));
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var recipe = await _db.Recipes
                .AsNoTracking()
                .Include(r => r.Category)
                .Include(r => r.Ratings)
                .Include(r => r.RecipeIngredients)
                    .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null) return NotFound();

            var vm = new RecipeDetailsVM
            {
                Recipe = recipe,
                AvgRating = recipe.Ratings.Count == 0 ? 0 : recipe.Ratings.Average(x => x.Value),
                RatingsCount = recipe.Ratings.Count
            };

            return View(vm);
        }

        public async Task<IActionResult> Create()
        {
            var vm = await BuildFormVMAsync(null);
            // default: one row
            vm.Ingredients.Add(new RecipeIngredientRowVM());
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeFormVM vm)
        {
            await FillOptionsAsync(vm);

            if (vm.Ingredients.Count == 0)
                ModelState.AddModelError("", "Add at least 1 ingredient.");

            if (!ModelState.IsValid) return View(vm);

            var recipe = new Recipe
            {
                Title = vm.Title,
                Description = vm.Description,
                Instructions = vm.Instructions,
                PrepTimeMinutes = vm.PrepTimeMinutes,
                Portions = vm.Portions,
                Difficulty = vm.Difficulty,
                CategoryId = vm.CategoryId
            };

            foreach (var row in vm.Ingredients)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    IngredientId = row.IngredientId,
                    Amount = row.Amount,
                    Unit = row.Unit
                });
            }

            _db.Recipes.Add(recipe);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = recipe.Id });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vm = await BuildFormVMAsync(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeFormVM vm)
        {
            if (vm.Id != id) return BadRequest();
            await FillOptionsAsync(vm);

            if (vm.Ingredients.Count == 0)
                ModelState.AddModelError("", "Add at least 1 ingredient.");

            if (!ModelState.IsValid) return View(vm);

            var recipe = await _db.Recipes
                .Include(r => r.RecipeIngredients)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null) return NotFound();

            recipe.Title = vm.Title;
            recipe.Description = vm.Description;
            recipe.Instructions = vm.Instructions;
            recipe.PrepTimeMinutes = vm.PrepTimeMinutes;
            recipe.Portions = vm.Portions;
            recipe.Difficulty = vm.Difficulty;
            recipe.CategoryId = vm.CategoryId;

            // Replace ingredients (simple & safe for student project)
            recipe.RecipeIngredients.Clear();
            foreach (var row in vm.Ingredients)
            {
                recipe.RecipeIngredients.Add(new RecipeIngredient
                {
                    RecipeId = recipe.Id,
                    IngredientId = row.IngredientId,
                    Amount = row.Amount,
                    Unit = row.Unit
                });
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var recipe = await _db.Recipes
                .AsNoTracking()
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null) return NotFound();
            return View(recipe);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _db.Recipes.FindAsync(id);
            if (recipe == null) return NotFound();

            _db.Recipes.Remove(recipe);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private async Task<RecipeFormVM?> BuildFormVMAsync(int? recipeId)
        {
            RecipeFormVM vm;

            if (recipeId == null)
            {
                vm = new RecipeFormVM();
            }
            else
            {
                var recipe = await _db.Recipes
                    .AsNoTracking()
                    .Include(r => r.RecipeIngredients)
                    .FirstOrDefaultAsync(r => r.Id == recipeId.Value);

                if (recipe == null) return null;

                vm = new RecipeFormVM
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Instructions = recipe.Instructions,
                    PrepTimeMinutes = recipe.PrepTimeMinutes,
                    Portions = recipe.Portions,
                    Difficulty = recipe.Difficulty,
                    CategoryId = recipe.CategoryId,
                    Ingredients = recipe.RecipeIngredients
                        .OrderBy(x => x.IngredientId)
                        .Select(x => new RecipeIngredientRowVM
                        {
                            IngredientId = x.IngredientId,
                            Amount = x.Amount,
                            Unit = x.Unit
                        })
                        .ToList()
                };
            }

            await FillOptionsAsync(vm);
            return vm;
        }

        private async Task FillOptionsAsync(RecipeFormVM vm)
        {
            vm.CategoryOptions = await _db.Categories.AsNoTracking()
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem(c.Name, c.Id.ToString()))
                .ToListAsync();

            vm.IngredientOptions = await _db.Ingredients.AsNoTracking()
                .OrderBy(i => i.Name)
                .Select(i => new SelectListItem(i.Name, i.Id.ToString()))
                .ToListAsync();
        }
    }
}
