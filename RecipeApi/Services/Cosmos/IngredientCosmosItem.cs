using Microsoft.Azure.CosmosRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Services.Cosmos
{
    public class IngredientCosmosItem : Item
    {
        public string Name { get; set; }
    }
}
