using Microsoft.AspNetCore.Mvc.Rendering;

namespace KsiazkaKucharska.VievModels
{
    public class RecipeIndexVM
    {
        public string? Q { get; set; }
        public int? CategoryId { get; set; }

        // Search by ingredients
        public List<int> IngredientIds { get; set; } = new();
        public bool MatchAllIngredients { get; set; } = false;

        public List<SelectListItem> CategoryOptions { get; set; } = new();
        public List<SelectListItem> IngredientOptions { get; set; } = new();

        public List<RecipeIndexRow> Results { get; set; } = new();
    }

    public class RecipeIndexRow
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string CategoryName { get; set; } = "";
        public int PrepTimeMinutes { get; set; }
        public double AvgRating { get; set; }
        public int RatingsCount { get; set; }
    }

}
