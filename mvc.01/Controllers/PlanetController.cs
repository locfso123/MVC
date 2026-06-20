using Microsoft.AspNetCore.Mvc;
using mvc01.Services;
using System.Xml.Linq;

namespace mvc01.Controllers
{
    [Route("he-mat-troi/[action]")]
    public class PlanetController : Controller
    {
        private readonly PlanetService _planetService;
        ILogger<PlanetController> _logger;

        public PlanetController(PlanetService planetService, ILogger<PlanetController> logger)
        {
            _planetService = planetService;
            _logger = logger;
        }

        [Route("danh-sach-cac-hanh-tinh.html")]
        public IActionResult Index()
        {
            return View();
        }

        [BindProperty(SupportsGet = true, Name = "action")]
        public string Name { get; set; }
        public IActionResult Mercury()
        {
            var planet = _planetService.Where(p=> p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }
        public IActionResult Venus()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }
        public IActionResult Earth()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }
        public IActionResult Mars()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        [HttpGet("/saomoc.html")]
        public IActionResult Jupiter()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }
        public IActionResult Saturn()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }
        public IActionResult Uranus()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        [Route("sao/[action]", Order = 3, Name = "Neptune3")]
        [Route("sao/[controller]/[action]", Order = 2, Name = "Neptune2")]
        [Route("sao/[controller]-[action].html", Order = 1, Name = "Neptune1")]
        public IActionResult Neptune()
        {
            var planet = _planetService.Where(p => p.Name == Name).FirstOrDefault();
            return View("Detail", planet);
        }

        [Route("/hanh-tinh/{id:int}")]
        public IActionResult PlanetInfo(int id)
        {
            var planet = _planetService.Where(p => p.Id == id).FirstOrDefault();
            return View("Detail", planet);
        }

    }
}
