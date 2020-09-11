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

        public async Task<IActionResult> Index()
        {
            var searchResponse = await _elasticClient.SearchAsync<Recipe>(s => s.Index("recipes"));

            return View(searchResponse.Documents);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
