using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeRating
{
    public class RecipeRating
    {
        public string RecipeId { get; set; }
        public short Rating { get; set; }
        public int TotalVoteCount { get; set; }
    }
}
