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
    public class RecipesController : ControllerBase
    {
        readonly IRepository<Recipe> _recipeRepository;
        readonly IRepository<Ingredient> _ingredientRepository;

        public RecipesController(IRepositoryFactory factory)
        {
            _recipeRepository = factory.RepositoryOf<Recipe>();
            _ingredientRepository = factory.RepositoryOf<Ingredient>();
        }

        /// <summary>
        /// Get all recipes.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetRecipes))]
        public async ValueTask<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var result = await _recipeRepository.GetAsync(x => x.Id != null);
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
                var result = await _recipeRepository.GetAsync(id);
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
                var result = await _recipeRepository.GetAsync(
                    r => r.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
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
            if(string.IsNullOrEmpty(request.Name))
            {
                return new ConflictResult();
            }

            var existing = await _recipeRepository.GetAsync(r => r.Name == request.Name);
            if(existing.Any())
            {
                return new ConflictResult();
            }

            var result = await _recipeRepository.CreateAsync(new Recipe
            {
                Name = request.Name
            });

            return new CreatedResult($"/api/recipes/{result.Id}", result);
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
                var existingRecipe = await _recipeRepository.GetAsync(id);

                var existingIngredients = await _ingredientRepository.GetAsync(x
                    => x.Name.Contains(request.IngredientName, StringComparison.OrdinalIgnoreCase));

                var newIngredient = !existingIngredients.Any()
                    ? await _ingredientRepository.CreateAsync(new Ingredient
                      {
                          Name = request.IngredientName
                      })
                    : existingIngredients.First();
                
                existingRecipe.Ingredients.Add(new MeasuredIngredient
                {
                    Ingredient = newIngredient,
                    UnitOfMeasure = request.UnitOfMeasure,
                    Amount = request.Amount
                });

                await _recipeRepository.UpdateAsync(existingRecipe);

                return new CreatedResult($"/api/recipes/{existingRecipe.Id}", existingRecipe);
            }
            catch
            {
                return new ConflictResult();
            }
        }
    }
}
