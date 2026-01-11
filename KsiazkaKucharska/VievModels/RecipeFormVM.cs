using System.ComponentModel.DataAnnotations;
using KsiazkaKucharska.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KsiazkaKucharska.VievModels
{
    public class RecipeIngredientRowVM
    {
        [Required]
        public int IngredientId { get; set; }

        [Range(0.01, 999999)]
        public decimal Amount { get; set; } = 1;

        [Required, StringLength(30)]
        public string Unit { get; set; } = "g";
    }

    public class RecipeFormVM
    {
        public int? Id { get; set; }

        [Required, StringLength(120)]
        public string Title { get; set; } = string.Empty;

        [StringLength(600)]
        public string? Description { get; set; }

        [Required]
        public string Instructions { get; set; } = string.Empty;

        [Range(1, 2000)]
        public int PrepTimeMinutes { get; set; } = 20;

        [Range(1, 50)]
        public int Portions { get; set; } = 2;

        public Difficulty Difficulty { get; set; } = Difficulty.Easy;

        [Required]
        public int CategoryId { get; set; }

        public List<RecipeIngredientRowVM> Ingredients { get; set; } = new();

        // For dropdowns
        public List<SelectListItem> CategoryOptions { get; set; } = new();
        public List<SelectListItem> IngredientOptions { get; set; } = new();
    }
}
