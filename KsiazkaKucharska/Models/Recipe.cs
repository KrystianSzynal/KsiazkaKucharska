using System.ComponentModel.DataAnnotations;

namespace KsiazkaKucharska.Models
{
    public class Recipe
    {
        public int Id { get; set; }

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

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Category (1..N)
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Many-to-many via join
        public List<RecipeIngredient> RecipeIngredients { get; set; } = new();

        // Ratings
        public List<Rating> Ratings { get; set; } = new();
    }
}
