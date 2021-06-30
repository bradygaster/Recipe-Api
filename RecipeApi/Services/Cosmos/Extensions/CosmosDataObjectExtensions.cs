using RecipeApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Services.Cosmos
{
    public static class RecipeCosmosItemExtensions
    {
        public static Recipe ToRecipe(this RecipeCosmosItem recipe)
        {
            return new Recipe
            {
                Id = recipe.Id,
                Ingredients = recipe.Ingredients,
                Name = recipe.Name
            };
        }

        public static RecipeCosmosItem FromRecipe(this Recipe recipe)
        {
            return new RecipeCosmosItem
            {
                Id = recipe.Id,
                Ingredients = recipe.Ingredients,
                Name = recipe.Name
            };
        }
    }

    public static class IngredientCosmosItemExtensions
    {
        public static Ingredient ToIngredient(this IngredientCosmosItem ingredient)
        {
            return new Ingredient
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };
        }

        public static IngredientCosmosItem FromIngredient(this Ingredient ingredient)
        {
            return new IngredientCosmosItem
            {
                Id = ingredient.Id,
                Name = ingredient.Name
            };
        }
    }
}
