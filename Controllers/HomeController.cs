using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Test()
        {
            var viewModel = new DtoA
            {
                Id = 3,
                DtoBs = new[]
                                        {
                                            new DtoB { Id = 4 },
                                            new DtoB
                                            {
                                                Id = 5,
                                                DtoCs = new[]
                                                        {
                                                            new DtoC { Id = 5 },
                                                            new DtoC { Id = 6 },
                                                        }
                                            },
                                        }
            };

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Test")]
        public IActionResult PostTest(DtoA dtoA)
        {
            if (!ModelState.IsValid)
            {
                var ModelStateErrors = ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(k => k.Key, k => k.Value.Errors.Select(e => e.ErrorMessage).ToArray());

                ViewData["Error"] = string.Join("<br>", ModelStateErrors.Select(d => $"\"{d.Key}\" : { string.Join("", d.Value)}"));
            }
            return View(dtoA);
        }

        public class DtoA
        {
            [Range(1, 2, ErrorMessage = "{0} 要介於 {1} 及 {2} 之間")]
            public int Id { get; set; }

            public DtoB[] DtoBs { get; set; }
        }


        public class DtoB
        {
            [Range(1, 2, ErrorMessage = "{0} 要介於 {1} 及 {2} 之間")]
            public int Id { get; set; }

            public DtoC[] DtoCs { get; set; }
        }

        public class DtoC
        {
            [Range(1, 2, ErrorMessage = "{0} 要介於 {1} 及 {2} 之間")]
            public int Id { get; set; }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
