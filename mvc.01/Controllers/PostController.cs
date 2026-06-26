/*using Microsoft.AspNetCore.Mvc;
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

            if (post != null)
            {
                return View(post);
            }

            // If not a post, check if it's a category and render category posts directly
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Slug == slug);

            if (category != null)
            {
                var posts = await _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.PostCategories)
                    .ThenInclude(pc => pc.Category)
                    .Where(p => p.PostCategories.Any(pc => pc.CategoryID == category.Id) && p.Published)
                    .OrderByDescending(p => p.DateCreated)
                    .ToListAsync();

                ViewData["CategoryTitle"] = category.Title;
                ViewData["CategorySlug"] = category.Slug;

                return View("Index", posts);
            }

            // Try to find category by partial match or without leading hyphen
            var normalizedSlug = slug.TrimStart('-');
            var categoryByNormalized = await _context.Categories
                .FirstOrDefaultAsync(c => c.Slug == normalizedSlug || c.Slug.Contains(slug));

            if (categoryByNormalized != null)
            {
                var posts = await _context.Posts
                    .Include(p => p.Author)
                    .Include(p => p.PostCategories)
                    .ThenInclude(pc => pc.Category)
                    .Where(p => p.PostCategories.Any(pc => pc.CategoryID == categoryByNormalized.Id) && p.Published)
                    .OrderByDescending(p => p.DateCreated)
                    .ToListAsync();

                ViewData["CategoryTitle"] = categoryByNormalized.Title;
                ViewData["CategorySlug"] = categoryByNormalized.Slug;

                return View("Index", posts);
            }

            // Log or return a more specific error message
            return NotFound($"Không tìm thấy bài viết hoặc danh mục với slug: {slug}");
        }

        // GET: Post/category/{slug}
        [HttpGet("/post/category/{slug}")]
        public async Task<IActionResult> Category(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return RedirectToAction(nameof(Index));
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.Slug == slug);

            if (category == null)
            {
                return NotFound();
            }

            var posts = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.PostCategories.Any(pc => pc.CategoryID == category.Id) && p.Published)
                .OrderByDescending(p => p.DateCreated)
                .ToListAsync();

            ViewData["CategoryTitle"] = category.Title;
            ViewData["CategorySlug"] = category.Slug;

            return View("Index", posts);
        }
    }
}
*/