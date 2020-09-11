using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ElasticsearchRecipes.Web.Models;

using Nest;

namespace ElasticsearchRecipes.Web.Controllers
{
    public class RecipesController : Controller
    {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<RecipesController> _logger;

        public RecipesController(IElasticClient elasticClient, ILogger<RecipesController> logger)
        {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string query)
        {
            var searchResponse = await _elasticClient.SearchAsync<Recipe>(s =>
            {
                s = s.Index("recipes");
                if (!string.IsNullOrWhiteSpace(query))
                    s = s
                        .Query(q => q
                                   .MultiMatch(m => m
                                                   .Query(query)
                                                   .Fields(f => f
                                                               .Field(p => p.Title)
                                                               .Field(p => p.Description)
                                                   )
                                   )
                        );
                return s;
            });

            var recipes = searchResponse.Hits.Select(h =>
            {
                h.Source.Id = h.Id;
                return h.Source;
            });

            return View(new RecipesViewModel
            {
                Query = query,
                Recipes = recipes
            });
        }

        [Route("{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var getResponse = await _elasticClient.GetAsync<Recipe>(id, g => g.Index("recipes"));

            return View(getResponse.Source);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
