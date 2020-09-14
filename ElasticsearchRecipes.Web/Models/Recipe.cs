using System.Collections.Generic;

using Nest;

namespace ElasticsearchRecipes.Web.Models
{
    public class Recipe
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Number(Name = "preparation_time_minutes")]
        public int PreparationTimeMinutes { get; set; }
        public IEnumerable<Ingredient> Ingredients { get; set; }
        public IEnumerable<string> Steps { get; set; }

        public class Ingredient
        {
            public string Name { get; set; }
            public string Quantity { get; set; }
        }
    }
}
