using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CosmosRepository;
using RecipeApi.Data;
using RecipeApi.ServiceModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        readonly IRepository<Recipe> _recipeRepository;
        readonly IRepository<Ingredient> _ingredientRepository;

        public IngredientsController(IRepositoryFactory factory)
        {
            _recipeRepository = factory.RepositoryOf<Recipe>();
            _ingredientRepository = factory.RepositoryOf<Ingredient>();
        }

        /// <summary>
        /// Get all of my ingredients.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetAllIngredients))]
        public async ValueTask<ActionResult<IEnumerable<Ingredient>>> GetAllIngredients()
        {
            var result = await _ingredientRepository.GetAsync(x => x.Id != null);
            return new JsonResult(result);
        }

        /// <summary>
        /// Get all recipes that contain a specific ingredient.
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        [HttpGet("Search/{ingredient}/Recipes", Name = nameof(SearchForRecipesIncludingIngredient))]
        public async ValueTask<ActionResult<IEnumerable<Recipe>>> SearchForRecipesIncludingIngredient(
            string ingredient)
        {
            var result = await _recipeRepository.GetAsync(
                r => r.Ingredients.Any(
                    i => i.Ingredient.Name.Contains(ingredient, StringComparison.OrdinalIgnoreCase)));
            return new JsonResult(result);
        }

        /// <summary>
        /// Search for an existing ingredient by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Search/{name}", Name = nameof(SearchForIngredient))]
        public async ValueTask<ActionResult<IEnumerable<Recipe>>> SearchForIngredient(
            string name)
        {
            try
            {
                var result = await _ingredientRepository.GetAsync(
                    i => i.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
                return new JsonResult(result);
            }
            catch
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Create a new ingredient.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = nameof(CreateIngredient))]
        public async ValueTask<ActionResult<Ingredient>> CreateIngredient(
            [FromBody] IngredientRequest request)
        {
            var existingIngredient = await _ingredientRepository.GetAsync(x => x.Name == request.IngredientName);
            if (existingIngredient.Any())
            {
                return new ConflictResult();
            }
            var result = await _ingredientRepository.CreateAsync(new Ingredient { Name = request.IngredientName });
            return new CreatedResult($"/api/ingredients/{result.Id}", result);
        }
    }
}
