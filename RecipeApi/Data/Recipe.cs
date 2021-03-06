﻿using Microsoft.Azure.CosmosRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeApi.Data
{
    public class Recipe
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<MeasuredIngredient> Ingredients { get; set; } = new List<MeasuredIngredient>();
    }
}
