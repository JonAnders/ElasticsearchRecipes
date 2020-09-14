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

        public async Task<IActionResult> Index(string query, string preptime)
        {
            // Is there a better way of building up an elasticsearch query? This is a bit messy...
            var searchResponse = await this._elasticClient.SearchAsync<Recipe>(s => s
                .Index("recipes")
                .Query(q => q
                    .Bool(b =>
                    {
                        if (!string.IsNullOrWhiteSpace(query))
                            b = b.Must(bq => bq
                                .MultiMatch(m => m
                                    .Query(query)
                                    .Fields(f => f
                                        .Field(p => p.Title)
                                        .Field(p => p.Description))));

                        switch (preptime)
                        {
                            case "15":
                                b = b.Filter(fq => fq
                                    .Range(nr => nr
                                        .Field(p => p.PreparationTimeMinutes)
                                        .LessThanOrEquals(15)));
                                break;
                            case "30":
                                b = b.Filter(fq => fq
                                    .Range(nr => nr
                                        .Field(p => p.PreparationTimeMinutes)
                                        .GreaterThanOrEquals(15)
                                        .LessThanOrEquals(30)));
                                break;
                            case "more":
                                b = b.Filter(fq => fq
                                    .Range(nr => nr
                                        .Field(p => p.PreparationTimeMinutes)
                                        .GreaterThanOrEquals(30)));
                                break;
                        }

                        return b;
                    })));

            var recipes = searchResponse.Hits.Select(h =>
            {
                h.Source.Id = h.Id;
                return h.Source;
            });

            return View(new RecipesViewModel
            {
                Query = query,
                Recipes = recipes,
                PrepTime = preptime
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
