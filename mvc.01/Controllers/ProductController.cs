/*using Microsoft.AspNetCore.Mvc;
using mvc01.Services;

namespace mvc01.Controllers
{
    [Area("ProductManage")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(ProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        [Route("cac-san-pham/{id?}")]
        public IActionResult Index()
        {
            var products = _productService.OrderBy(p => p.Name).ToList();

            return View(products);
        }
    }
}*/