using Microsoft.AspNetCore.Http;
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
        public IngredientsController(IRepository<Recipe> recipeRepository,
            IRepository<Ingredient> ingredientRepository)
        {
            RecipeRepository = recipeRepository;
            IngredientRepository = ingredientRepository;
        }

        public IRepository<Recipe> RecipeRepository { get; }
        public IRepository<Ingredient> IngredientRepository { get; }

        /// <summary>
        /// Get all ingredients.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetAllIngredients))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetAllIngredients()
        {
            var result = await IngredientRepository.GetAsync(x => x.Id != null);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Get all recipes that contain a specific ingredient.
        /// </summary>
        /// <param name="ingredient"></param>
        /// <returns></returns>
        [HttpGet("Search/{ingredient}/Recipes", Name = nameof(SearchForRecipesIncludingIngredient))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchForRecipesIncludingIngredient(string ingredient)
        {
            var result = await RecipeRepository.GetAsync(r => r.Ingredients.Any(i => i.Ingredient.Name.Contains(ingredient, StringComparison.OrdinalIgnoreCase)));
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Search for an existing ingredient by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Search/{name}", Name = nameof(SearchForIngredient))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchForIngredient(string name)
        {
            try
            {
                var result = await IngredientRepository.GetAsync(x => x.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
                return new OkObjectResult(result);
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
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<Ingredient>> CreateIngredient(IngredientRequest request)
        {
            var existingIngredient = await IngredientRepository.GetAsync(x => x.Name == request.IngredientName);
            if (existingIngredient.Any())
            {
                return new ConflictResult();
            }
            var result = await IngredientRepository.CreateAsync(new Ingredient { Name = request.IngredientName });
            return new CreatedResult($"/api/ingredients/{result.Id}", result);
        }
    }
}
