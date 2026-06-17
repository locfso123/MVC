using Microsoft.AspNetCore.Mvc;
using mvc._01.Services;

namespace mvc._01.Controllers
{
    public class FirstController : Controller
    {
        private readonly ILogger<FirstController> _logger;
        private readonly ProductService _productService;
        public FirstController(ILogger<FirstController> logger, ProductService productService) 
        { 
            _logger = logger;
            _productService = productService;
        }

        public string Index()
        {
            _logger.LogInformation("Index Action");

            return "Hello from FirstController.Index";
        }

        public IActionResult Bird()
        {
            string filePath = Path.Combine(Program.ContentRootPath, "Files", "images.webp");
            var bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, "image/webp");
        }

        public IActionResult IphonePrice()
        {
            return Json(new
            {
                productName = "Iphon X",
                price = 1000
            });
        }

        public IActionResult privacy()
        {
            var url = Url.Action("Privacy", "Home");
            _logger.LogInformation("Chuyen huong den: " + url);
            return LocalRedirect(url);
        }

        public IActionResult HelloView(string username)
        {
            if (string.IsNullOrEmpty(username))
                username = "khach";

            return View("xinchao3", username);
        }

        [TempData]
        public string StatusMessage { get; set;  }

        [AcceptVerbs("POST", "GET")]
        public IActionResult ViewProduct(int?id)
        {
            var product = _productService.Where(p => p.Id == id).FirstOrDefault();
            if (product == null)
            {
                /*TempData["StatusMessage"] = "San pham ban yeu cau khong co";*/
                StatusMessage = "San pham ban yeu cau khong co";
                return Redirect(Url.Action("Index", "Home"));
            }

            /*return View(product);*/
            /*this.ViewData["product"] = product;*/
            this.ViewData["Title"] = product.Name;

            ViewBag.product = product;

            return View("ViewProduct2");
        }
    }
}
