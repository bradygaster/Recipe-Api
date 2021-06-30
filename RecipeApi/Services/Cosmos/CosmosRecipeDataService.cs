using Microsoft.Azure.CosmosRepository;
using RecipeApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Services.Cosmos
{
    public class CosmosRecipeDataService : IRecipeDataService
    {
        readonly IRepository<RecipeCosmosItem> _recipeRepository;
        readonly IRepository<IngredientCosmosItem> _ingredientRepository;

        public CosmosRecipeDataService(IRepositoryFactory factory)
        {
            _recipeRepository = factory.RepositoryOf<RecipeCosmosItem>();
            _ingredientRepository = factory.RepositoryOf<IngredientCosmosItem>();
        }

        public async Task<Recipe> AddIngredientToRecipe(string recipeId, MeasuredIngredient ingredient)
        {
            var existingRecipe = await _recipeRepository.GetAsync(recipeId);

            var existingIngredients = await _ingredientRepository.GetAsync(x
                => x.Name.Contains(ingredient.Ingredient.Name, StringComparison.OrdinalIgnoreCase));

            var newIngredient = !existingIngredients.Any()
                ? await _ingredientRepository.CreateAsync(new IngredientCosmosItem
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = ingredient.Ingredient.Name
                })
                : existingIngredients.First();

            existingRecipe.Ingredients.Add(new MeasuredIngredient
            {
                Amount = ingredient.Amount,
                Ingredient = newIngredient.ToIngredient(),
                UnitOfMeasure = ingredient.UnitOfMeasure
            });

            await _recipeRepository.UpdateAsync(existingRecipe);

            return existingRecipe.ToRecipe();
        }

        public async Task<Recipe> CreateRecipe(Recipe recipe)
        {
            var existing = await _recipeRepository.GetAsync(r => r.Name == recipe.Name);

            if (existing.Any())
            {
                throw new ArgumentException("A recipe already exists with that name. Please give the recipe a distinctive name.");
            }

            var result = await _recipeRepository.CreateAsync(new RecipeCosmosItem
            {
                Name = recipe.Name
            });

            return result.ToRecipe();
        }

        public async Task<IEnumerable<Ingredient>> GetIngredients()
        {
            var result = await _ingredientRepository.GetAsync(_ => _.Id != null);
            return result.Select(_ => _.ToIngredient()).OrderBy(_ => _.Name).AsEnumerable();
        }

        public async Task<Recipe> GetRecipe(string id)
        {
            var result = await _recipeRepository.GetAsync(id);
            return result.ToRecipe();
        }

        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            var result = await _recipeRepository.GetAsync(x => x.Id != null);
            return result.ToList().Select(_ => _.ToRecipe()).OrderBy(x => x.Name).AsEnumerable();
        }

        public async Task<IEnumerable<Recipe>> SearchForRecipes(string whereNameContains)
        {
            var result = await _recipeRepository.GetAsync(
                    r => r.Name.Contains(whereNameContains, StringComparison.OrdinalIgnoreCase));
            return result.Select(_ => _.ToRecipe()).OrderBy(_ => _.Name).AsEnumerable();
        }

        public async Task<IEnumerable<Recipe>> SearchForRecipesIncludingIngredient(Ingredient ingredient)
        {
            var result = await _recipeRepository.GetAsync(
               r => r.Ingredients.Any(
                   i => i.Ingredient.Name.Contains(ingredient.Name, StringComparison.OrdinalIgnoreCase)));

            return result.Select(_ => _.ToRecipe()).OrderBy(_ => _.Name).AsEnumerable();
        }
    }
}
