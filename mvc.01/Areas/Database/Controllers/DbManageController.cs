using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc01.Models;

namespace mvc01.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AppDbContext _dbContext;

        public DbManageController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {   
            return View();
        }

        [HttpGet]
        public IActionResult DeleteDb()
        {
            return View();
        }

        [TempData]
        public string StatusMessage {  get; set; }

        [HttpPost]
        public async Task<IActionResult> DeleteDbAsync()
        {
            var success = await _dbContext.Database.EnsureDeletedAsync();

            StatusMessage = success ? "Xoa Db thanh cong" : "Khong xoa duoc Db";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Migrate()
        {
            await _dbContext.Database.MigrateAsync();

            StatusMessage = "Cap nhap Db thanh cong";

            return RedirectToAction(nameof(Index));
        }


    }
}
