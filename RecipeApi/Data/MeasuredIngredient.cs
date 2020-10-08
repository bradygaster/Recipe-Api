using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Data
{
    public class MeasuredIngredient
    {
        public Ingredient Ingredient { get; set; }
        public float Amount { get; set; }
        public string UnitOfMeasure { get; set; }
    }
}
