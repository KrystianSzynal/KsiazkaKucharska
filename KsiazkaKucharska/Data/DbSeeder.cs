using KsiazkaKucharska.Models;
using Microsoft.EntityFrameworkCore;

namespace KsiazkaKucharska.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (!await db.Categories.AnyAsync())
            {
                db.Categories.AddRange(
                    new Category { Name = "Breakfast" },
                    new Category { Name = "Dinner" },
                    new Category { Name = "Dessert" },
                    new Category { Name = "Soup" }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.Ingredients.AnyAsync())
            {
                db.Ingredients.AddRange(
                    new Ingredient { Name = "Egg" },
                    new Ingredient { Name = "Milk" },
                    new Ingredient { Name = "Flour" },
                    new Ingredient { Name = "Sugar" },
                    new Ingredient { Name = "Butter" },
                    new Ingredient { Name = "Salt" }
                );
                await db.SaveChangesAsync();
            }

            if (!await db.Recipes.AnyAsync())
            {
                if (!await db.Recipes.AnyAsync())
                {
                    // ===== CATEGORIES =====
                    var breakfast = await db.Categories.FirstAsync(c => c.Name == "Breakfast");
                    var dinner = await db.Categories.FirstAsync(c => c.Name == "Dinner");
                    var dessert = await db.Categories.FirstAsync(c => c.Name == "Dessert");

                    // ===== INGREDIENTS =====
                    var egg = await db.Ingredients.FirstAsync(i => i.Name == "Egg");
                    var milk = await db.Ingredients.FirstAsync(i => i.Name == "Milk");
                    var flour = await db.Ingredients.FirstAsync(i => i.Name == "Flour");
                    var sugar = await db.Ingredients.FirstAsync(i => i.Name == "Sugar");
                    var butter = await db.Ingredients.FirstAsync(i => i.Name == "Butter");
                    var salt = await db.Ingredients.FirstAsync(i => i.Name == "Salt");

                    // Dinner extras (add if missing)
                    Ingredient chicken, rice, onion, garlic, tomato, pasta, cheese, beef, potato, carrot, oil;

                    chicken = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Chicken breast")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Chicken breast" }).Entity;
                    rice = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Rice")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Rice" }).Entity;
                    onion = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Onion")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Onion" }).Entity;
                    garlic = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Garlic")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Garlic" }).Entity;
                    tomato = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Tomato")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Tomato" }).Entity;
                    pasta = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Pasta")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Pasta" }).Entity;
                    cheese = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Cheese")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Cheese" }).Entity;
                    beef = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Beef")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Beef" }).Entity;
                    potato = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Potato")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Potato" }).Entity;
                    carrot = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Carrot")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Carrot" }).Entity;
                    oil = await db.Ingredients.FirstOrDefaultAsync(i => i.Name == "Cooking oil")
                        ?? db.Ingredients.Add(new Ingredient { Name = "Cooking oil" }).Entity;

                    await db.SaveChangesAsync();

                    // ===== PAN FRIED CHICKEN =====
                    var chickenDinner = new Recipe
                    {
                        Title = "Pan-Fried Chicken Breast",
                        Description = "Simple and juicy chicken breast fried on a pan.",
                        Instructions =
                            "1. Season the chicken breast with salt.\n" +
                            "2. Heat oil in a pan over medium heat.\n" +
                            "3. Fry chicken for 6–7 minutes per side.\n" +
                            "4. Serve hot with rice or potatoes.",
                        PrepTimeMinutes = 25,
                        Portions = 2,
                        Difficulty = Difficulty.Easy,
                        CategoryId = dinner.Id
                    };

                    chickenDinner.RecipeIngredients.Add(new RecipeIngredient { IngredientId = chicken.Id, Amount = 2, Unit = "pcs" });
                    chickenDinner.RecipeIngredients.Add(new RecipeIngredient { IngredientId = oil.Id, Amount = 2, Unit = "tbsp" });
                    chickenDinner.RecipeIngredients.Add(new RecipeIngredient { IngredientId = salt.Id, Amount = 1, Unit = "pinch" });

                    db.Recipes.Add(chickenDinner);

                    // ===== CHICKEN WITH RICE =====
                    var chickenRice = new Recipe
                    {
                        Title = "Chicken with Rice",
                        Description = "Classic dinner with chicken breast and steamed rice.",
                        Instructions =
                            "1. Cook rice according to package instructions.\n" +
                            "2. Fry chicken breast on oil until golden.\n" +
                            "3. Serve chicken with rice.",
                        PrepTimeMinutes = 30,
                        Portions = 2,
                        Difficulty = Difficulty.Easy,
                        CategoryId = dinner.Id
                    };

                    chickenRice.RecipeIngredients.Add(new RecipeIngredient { IngredientId = chicken.Id, Amount = 2, Unit = "pcs" });
                    chickenRice.RecipeIngredients.Add(new RecipeIngredient { IngredientId = rice.Id, Amount = 200, Unit = "g" });
                    chickenRice.RecipeIngredients.Add(new RecipeIngredient { IngredientId = oil.Id, Amount = 1, Unit = "tbsp" });

                    db.Recipes.Add(chickenRice);

                    // ===== SPAGHETTI BOLOGNESE =====
                    var spaghetti = new Recipe
                    {
                        Title = "Spaghetti Bolognese",
                        Description = "Italian-style pasta with beef and tomato sauce.",
                        Instructions =
                            "1. Cook pasta until al dente.\n" +
                            "2. Fry onion and garlic in oil.\n" +
                            "3. Add beef and fry until browned.\n" +
                            "4. Add tomatoes and simmer.\n" +
                            "5. Serve sauce with pasta.",
                        PrepTimeMinutes = 40,
                        Portions = 3,
                        Difficulty = Difficulty.Medium,
                        CategoryId = dinner.Id
                    };

                    spaghetti.RecipeIngredients.Add(new RecipeIngredient { IngredientId = pasta.Id, Amount = 300, Unit = "g" });
                    spaghetti.RecipeIngredients.Add(new RecipeIngredient { IngredientId = beef.Id, Amount = 400, Unit = "g" });
                    spaghetti.RecipeIngredients.Add(new RecipeIngredient { IngredientId = onion.Id, Amount = 1, Unit = "pc" });
                    spaghetti.RecipeIngredients.Add(new RecipeIngredient { IngredientId = garlic.Id, Amount = 2, Unit = "cloves" });
                    spaghetti.RecipeIngredients.Add(new RecipeIngredient { IngredientId = tomato.Id, Amount = 400, Unit = "g" });

                    db.Recipes.Add(spaghetti);

                    // ===== BEEF STEW =====
                    var beefStew = new Recipe
                    {
                        Title = "Beef Stew",
                        Description = "Slow cooked beef stew with vegetables.",
                        Instructions =
                            "1. Cut beef into cubes.\n" +
                            "2. Fry beef on oil.\n" +
                            "3. Add vegetables and water.\n" +
                            "4. Simmer for 90 minutes until tender.",
                        PrepTimeMinutes = 120,
                        Portions = 4,
                        Difficulty = Difficulty.Hard,
                        CategoryId = dinner.Id
                    };

                    beefStew.RecipeIngredients.Add(new RecipeIngredient { IngredientId = beef.Id, Amount = 600, Unit = "g" });
                    beefStew.RecipeIngredients.Add(new RecipeIngredient { IngredientId = potato.Id, Amount = 3, Unit = "pcs" });
                    beefStew.RecipeIngredients.Add(new RecipeIngredient { IngredientId = carrot.Id, Amount = 2, Unit = "pcs" });
                    beefStew.RecipeIngredients.Add(new RecipeIngredient { IngredientId = onion.Id, Amount = 1, Unit = "pc" });

                    db.Recipes.Add(beefStew);

                    await db.SaveChangesAsync();

                    // ===== RATINGS =====
                    db.Ratings.AddRange(
                        new Rating { RecipeId = chickenDinner.Id, Value = 5 },
                        new Rating { RecipeId = chickenRice.Id, Value = 4 },
                        new Rating { RecipeId = spaghetti.Id, Value = 5 },
                        new Rating { RecipeId = beefStew.Id, Value = 4 }
                    );

                    await db.SaveChangesAsync();
                }


            }
        }
    }
}
