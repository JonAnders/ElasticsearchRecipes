using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace ElasticsearchRecipes.Web.Models
{
    public class RecipesViewModel
    {
        public IEnumerable<Recipe> Recipes { get; set; }
        public string Query { get; set; }
        public string PrepTime { get; set; }
        public IEnumerable<SelectListItem> PrepTimeListItems => new List<SelectListItem>
        {
            new SelectListItem("Any", ""),
            new SelectListItem("Less than 15 minutes", "15"),
            new SelectListItem("15 - 30 minutes", "30"),
            new SelectListItem("More than 30 minutes", "more")
        };
    }
}
