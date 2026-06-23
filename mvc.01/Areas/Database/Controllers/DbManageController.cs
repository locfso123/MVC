using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc01.Data;
using mvc01.Models;

namespace mvc01.Areas.Database.Controllers
{
    [Area("Database")]
    [Route("/database-manage/[action]")]
    public class DbManageController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbManageController(AppDbContext dbContext, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
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
        public string StatusMessage { get; set; }

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

        public async Task<IActionResult> SeedDataAsync()
        {
            var rolenames = typeof(RoleName).GetFields().ToList();
            foreach (var r in rolenames)
            {
                string rolename = (string)r.GetRawConstantValue();
                var rfound = await _roleManager.FindByIdAsync(rolename);
                if (rfound == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole(rolename));
                }   
            }

            var useradmin = await _userManager.FindByEmailAsync("admin");
            if (useradmin == null)
            {
                useradmin = new AppUser()
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };

                await _userManager.CreateAsync(useradmin, "123456");
                await _userManager.AddToRoleAsync(useradmin, RoleName.Administrator);
            }

            StatusMessage = "Vua seed Database";

            return RedirectToAction("Index");
        }
    }
}

