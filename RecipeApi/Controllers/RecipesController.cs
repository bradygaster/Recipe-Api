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
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeDataService _recipeDataService;

        public RecipesController(IRepositoryFactory factory, IRecipeDataService recipeDataService)
        {
            _recipeDataService = recipeDataService;
        }

        /// <summary>
        /// Get all recipes.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetRecipes))]
        public async ValueTask<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var result = await _recipeDataService.GetRecipes();
            return new JsonResult(result);
        }

        /// <summary>
        /// Get a specific recipe by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetRecipe))]
        public async ValueTask<ActionResult<Recipe>> GetRecipe(string id)
        {
            try
            {
                var result = await _recipeDataService.GetRecipe(id);
                return new JsonResult(result);
            }
            catch
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Search recipes by name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("Search/{name}", Name = nameof(SearchRecipesByName))]
        public async ValueTask<ActionResult<IEnumerable<Recipe>>> SearchRecipesByName(string name)
        {
            try
            {
                var result = await _recipeDataService.SearchForRecipes(name);
                return new JsonResult(result);
            }
            catch
            {
                return new NotFoundResult();
            }
        }

        /// <summary>
        /// Create a recipe.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost(Name = nameof(CreateRecipe))]
        public async ValueTask<ActionResult<Recipe>> CreateRecipe(
            [FromBody] NewRecipeRequest request)
        {
            try
            {
                var result = await _recipeDataService.CreateRecipe(new Recipe { Name = request.Name });
                return new CreatedResult($"/api/recipes/{result.Id}", result);
            }
            catch(ArgumentException ex)
            {
                return new ConflictObjectResult(ex.Message);
            }
            catch
            {
                return new StatusCodeResult(500);
            }
        }

        /// <summary>
        /// Add an ingredient to an existing recipe.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}", Name = nameof(AddIngredientToRecipe))]
        public async ValueTask<ActionResult<Recipe>> AddIngredientToRecipe(
            string id,
            [FromBody] AddIngredientToRecipeRequest request)
        {
            try
            {
                var recipe = await _recipeDataService.AddIngredientToRecipe(id, new MeasuredIngredient
                {
                    Amount = request.Amount,
                    Ingredient = new Ingredient
                    {
                        Name = request.IngredientName
                    },
                    UnitOfMeasure = request.UnitOfMeasure
                });

                return new CreatedResult($"/api/recipes/{recipe.Id}", recipe);
            }
            catch
            {
                return new ConflictResult();
            }
        }
    }
}
