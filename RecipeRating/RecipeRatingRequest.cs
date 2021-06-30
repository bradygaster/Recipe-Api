using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeRating
{
    public class RecipeRatingRequest
    {
        public string RecipeId { get; set; }
        public short Rating { get; set; }
    }
}
