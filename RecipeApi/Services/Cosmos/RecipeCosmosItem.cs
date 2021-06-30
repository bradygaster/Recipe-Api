using Microsoft.Azure.CosmosRepository;
using RecipeApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Services.Cosmos
{
    public class RecipeCosmosItem : Item
    {
        public string Name { get; set; }
        public List<MeasuredIngredient> Ingredients { get; set; } = new List<MeasuredIngredient>();
    }
}
