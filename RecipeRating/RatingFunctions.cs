using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace RecipeRating
{
    public static class RatingFunctions
    {
        [FunctionName(nameof(SubmitRating))]
        [OpenApiOperation(operationId: nameof(SubmitRating),
          Visibility = OpenApiVisibilityType.Important)
        ]
        [OpenApiRequestBody("application/json",
            typeof(RecipeRatingRequest),
            Description = "Submit a new recipe rating.")]
        [OpenApiResponseWithoutBody(HttpStatusCode.OK)]
        public static async Task<IActionResult> SubmitRating(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = String.Empty;

            using (StreamReader streamReader = new StreamReader(req.Body))
            {
                requestBody = await streamReader.ReadToEndAsync();
            }

            var request = JsonConvert.DeserializeObject<RecipeRatingRequest>(requestBody);

            return new OkResult();
        }

        [FunctionName(nameof(GetRatingForRecipe))]
        [OpenApiParameter("recipeId",
            Summary = "The identifier of the recipe to be rated.",
            In = ParameterLocation.Query,
            Type = typeof(string))]
        [OpenApiOperation(operationId: nameof(GetRatingForRecipe),
          Visibility = OpenApiVisibilityType.Important)
        ]
        [OpenApiResponseWithBody(HttpStatusCode.OK,
            "application/json",
            typeof(RecipeRating))]
        public static IActionResult GetRatingForRecipe(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string recipeId = req.Query["recipeId"];

            return new OkObjectResult(new RecipeRating
            {
                RecipeId = recipeId,
                Rating = 4,
                TotalVoteCount = 42
            });
        }
    }
}

