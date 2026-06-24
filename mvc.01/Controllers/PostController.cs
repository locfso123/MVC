using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mvc01.Data;
using mvc01.Models;
using mvc01.Models.Blog;

namespace mvc01.Controllers
{
    public class PostController : Controller
    {
        private readonly AppDbContext _context;

        public PostController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        // GET: Post
        [HttpGet("/post")]
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.Published)
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();

            return View(posts);
        }

        // GET: Post/{slug}
        [HttpGet("/post/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return RedirectToAction(nameof(Index));
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.Published);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }
    }
}
