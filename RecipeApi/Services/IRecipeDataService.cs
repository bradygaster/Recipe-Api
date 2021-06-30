using RecipeApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Services
{
    public interface IRecipeDataService
    {
        Task<IEnumerable<Recipe>> GetRecipes();
        Task<Recipe> GetRecipe(string id);
        Task<Recipe> CreateRecipe(Recipe recipe);
        Task<IEnumerable<Ingredient>> GetIngredients();
        Task<IEnumerable<Recipe>> SearchForRecipesIncludingIngredient(Ingredient ingredient);
        Task<IEnumerable<Recipe>> SearchForRecipes(string whereNameContains);
        Task<Recipe> AddIngredientToRecipe(string recipeId, MeasuredIngredient ingredient);

    }
}
