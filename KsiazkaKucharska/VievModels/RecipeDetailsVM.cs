using KsiazkaKucharska.Models;

namespace KsiazkaKucharska.VievModels
{
    public class RecipeDetailsVM
    {
        public Recipe Recipe { get; set; } = default!;
        public double AvgRating { get; set; }
        public int RatingsCount { get; set; }
    }
}
