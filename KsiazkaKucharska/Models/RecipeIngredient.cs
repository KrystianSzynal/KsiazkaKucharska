using System.ComponentModel.DataAnnotations;

namespace KsiazkaKucharska.Models
{
    public class RecipeIngredient
    {
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        public int IngredientId { get; set; }
        public Ingredient? Ingredient { get; set; }
        [Range(0.01, 999999)]
        public decimal Amount { get; set; }
        [Required, StringLength(30)]
        public string Unit { get; set; } = "g";
    }
}
