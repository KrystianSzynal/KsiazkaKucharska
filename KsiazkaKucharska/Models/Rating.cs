using System.ComponentModel.DataAnnotations;

namespace KsiazkaKucharska.Models
{
    public class Rating
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }

        [Range(1, 5)]
        public int Value { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
