using Microsoft.Azure.CosmosRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Data
{
    public class Recipe : Item
    {
        public string Name { get; set; }
        public List<string> Steps { get; set; } = new List<string>();
        public List<MeasuredIngredient> Ingredients { get; set; } = new List<MeasuredIngredient>();
    }
}
