using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Nest;

namespace ElasticsearchRecipes.Web.Models
{
    public class Recipe
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Number(Name = "preparation_time_minutes")]
        public int PreparationTimeMinutes { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }

        public class Ingredient
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
        }
    }
}
