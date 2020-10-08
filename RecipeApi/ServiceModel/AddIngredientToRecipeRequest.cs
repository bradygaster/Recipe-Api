using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.ServiceModel
{
    public class AddIngredientToRecipeRequest
    {
        public string IngredientName { get; set; }
        public string UnitOfMeasure { get; set; }
        public float Amount { get; set; }
    }
}
