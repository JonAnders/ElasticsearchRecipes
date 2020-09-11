using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElasticsearchRecipes.Web.Models
{
    public class RecipesViewModel
    {
        public IEnumerable<Recipe> Recipes { get; set; }
        public string Query { get; set; }
    }
}
