using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CosmosRepository;
using RecipeApi.Data;
using RecipeApi.ServiceModel;
using RecipeApi.Services;
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
        private readonly IRecipeDataService _recipeDataService;

        public IngredientsController(IRecipeDataService recipeDataService)
        {
            _recipeDataService = recipeDataService;
        }

        /// <summary>
        /// Get all the ingredients.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetAllIngredients))]
        public async ValueTask<ActionResult<IEnumerable<Ingredient>>> GetAllIngredients()
        {
            var result = await _recipeDataService.GetIngredients();
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
            var result = await _recipeDataService.SearchForRecipesIncludingIngredient(new Ingredient { Name = ingredient });
            return new JsonResult(result);
        }
    }
}
