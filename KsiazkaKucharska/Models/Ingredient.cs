using System.ComponentModel.DataAnnotations;

namespace KsiazkaKucharska.Models
{
    public class Ingredient
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Name { get; set; } = string.Empty;

        public List<RecipeIngredient> RecipeIngredients { get; set; } = new();
    }
}
