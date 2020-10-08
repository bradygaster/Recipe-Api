using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.CosmosRepository;
using RecipeApi.Data;
using RecipeApi.ServiceModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        public RecipesController(IRepository<Recipe> recipeRepository,
            IRepository<Ingredient> ingredientRepository)
        {
            RecipeRepository = recipeRepository;
            IngredientRepository = ingredientRepository;
        }

        public IRepository<Recipe> RecipeRepository { get; }
        public IRepository<Ingredient> IngredientRepository { get; }

        /// <summary>
        /// Get all recipes.
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = nameof(GetRecipes))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipes()
        {
            var result = await RecipeRepository.GetAsync(x => x.Id != null);
            return new OkObjectResult(result);
        }

        /// <summary>
        /// Get a specific recipe by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = nameof(GetRecipe))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipe(string id)
        {
            try
            {
                var result = await RecipeRepository.GetAsync(id);
                return new OkObjectResult(result);
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
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<IEnumerable<Recipe>>> SearchRecipesByName(string name)
        {
            try
            {
                var result = await RecipeRepository.GetAsync(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                return new OkObjectResult(result);
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
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<Recipe>> CreateRecipe(NewRecipeRequest request)
        {
            var existing = await RecipeRepository.GetAsync(x => x.Name == request.Name);
            if(existing.Any())
            {
                return new ConflictResult();
            }

            var result = await RecipeRepository.CreateAsync(new Recipe
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
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<ActionResult<Recipe>> AddIngredientToRecipe(
            string id, 
            [FromBody] AddIngredientToRecipeRequest request
            )
        {
            try
            {
                var existingRecipe = await RecipeRepository.GetAsync(id);

                var existingIngredients = await IngredientRepository.GetAsync(x
                    => x.Name.Contains(request.IngredientName, StringComparison.OrdinalIgnoreCase));

                Ingredient newIngredient = null;

                if (!existingIngredients.Any())
                {
                    newIngredient = await IngredientRepository.CreateAsync(new Ingredient
                    {
                        Name = request.IngredientName
                    });
                }
                else
                {
                    newIngredient = existingIngredients.First();
                }

                existingRecipe.Ingredients.Add(new MeasuredIngredient
                {
                    Ingredient = newIngredient,
                    UnitOfMeasure = request.UnitOfMeasure,
                    Amount = request.Amount
                });

                await RecipeRepository.UpdateAsync(existingRecipe);

                return new CreatedResult($"/api/recipes/{existingRecipe.Id}", existingRecipe);
            }
            catch
            {
                return new ConflictResult();
            }
        }
    }
}
