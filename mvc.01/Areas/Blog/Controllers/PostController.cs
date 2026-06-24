using App.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc01.Areas.Blog.Models;
using mvc01.Data;
using mvc01.Models;
using mvc01.Models.Blog;

namespace mvc01.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("admin/blog/post/[action]/{id?}")]   
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class PostController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public PostController(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Post
        [HttpGet("/admin/post")]
        public async Task<IActionResult> Index([FromQuery(Name= "p")] int currentPage ,int pagesize)
        {
            var posts = _context.Posts
                .Include(p => p.Author)
                .OrderByDescending(p=>p.DateUpdated);

            int totalPosts = await posts.CountAsync();
            if (pagesize <= 0) pagesize = 10;
            int countPages = (int)Math.Ceiling((double)totalPosts / pagesize);

            if (currentPage > countPages) currentPage = countPages;
            if (currentPage < 1) currentPage = 1;

            var pagingModel = new PagingModel()
            {
                countpages = countPages,
                currentpage = currentPage,
                generateUrl = (pageNumber) => Url.Action("Index", new
                {
                    p = pageNumber,
                    pagesize = pagesize
                })
            };

            ViewBag.pagingModel = pagingModel;
            ViewBag.totalPosts = totalPosts;

            ViewBag.postInPage = (currentPage - 1) * pagesize;

            var postsInPage = await posts.Skip((currentPage - 1) * pagesize)
                        .Take(pagesize)
                        .Include(p => p.PostCategories)
                        .ThenInclude(pc=>pc.Category)
                        .ToListAsync();

            return View(postsInPage);

            /*model.totalUsers = await qr.CountAsync();
            model.countPages = (int)Math.Ceiling((double)model.totalUsers / model.ITEMS_PER_PAGE);

            if (model.currentPage < 1)
                model.currentPage = 1;
            if (model.currentPage > model.countPages)
                model.currentPage = model.countPages;

            var qr1 = qr.Skip((model.currentPage - 1) * model.ITEMS_PER_PAGE)
                        .Take(model.ITEMS_PER_PAGE)
                        .Select(u => new UserAndRole()
                        {
                            Id = u.Id,
                            UserName = u.UserName,
                        });

            return View(await posts.ToListAsync());*/
        }

        // GET: Post/Details/5
        [HttpGet("/admin/post/detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.PostCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
        [HttpGet("/admin/post/create")]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title");
            return View();
        }

        // POST: Post/Create
        [HttpPost("/admin/post/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published,CategoryIDs")]  CreatPostModel postModel, int[] selectedCategories)
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title", selectedCategories);

            if (ModelState.IsValid)
            {
                /*if (!string.IsNullOrEmpty(postModel.Slug) && await _context.Posts.AnyAsync(p => p.Slug == postModel.Slug))
                {
                    ModelState.AddModelError("Slug", "Nhap chuoi url khac");
                    return View(postModel);
                }*/

                var user = await _userManager.GetUserAsync(this.User);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                // Map CreatPostModel to Post
                var post = new Post
                {
                    Title = postModel.Title,
                    Description = postModel.Description,
                    Slug = postModel.Slug,
                    Content = postModel.Content,
                    Published = postModel.Published,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    AuthorId = user.Id
                };

                if (string.IsNullOrEmpty(post.Slug))
                {
                    post.Slug = AppUtilities.GenerateSlug(post.Title);
                    //post.Slug = GenerateSlug(post.Title);
                }

                if (!string.IsNullOrEmpty(postModel.Slug) && await _context.Posts.AnyAsync(p => p.Slug == postModel.Slug))
                {
                    ModelState.AddModelError("Slug", "Nhap chuoi url khac");
                    return View(postModel);
                }

                _context.Add(post);
                await _context.SaveChangesAsync();

                // Use CategoryIDs from the model instead of selectedCategories parameter
                if (postModel.CategoryIDs != null && postModel.CategoryIDs.Length > 0)
                {
                    foreach (var CateId in postModel.CategoryIDs)
                    {
                        _context.Add(new PostCategory()
                        {
                            CategoryID = CateId,
                            PostID = post.PostId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                StatusMessage = "Vua tao xong bai viet moi";

                return RedirectToAction(nameof(Index));
            }

            return View(postModel);
        }

        // GET: Post/Edit/5
        [HttpGet("/admin/post/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.PostCategories)
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            var postEdit = new CreatPostModel()
            {
                PostId = post.PostId,
                Title = post.Title,
                Content = post.Content,
                Description = post.Description,
                Slug = post.Slug,
                Published = post.Published,
                CategoryIDs = post.PostCategories.Select(pc => pc.CategoryID).ToArray()
            };

            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title", postEdit.CategoryIDs);

            return View(postEdit);
        }

        // POST: Post/Edit/5
        [HttpPost("/admin/post/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,Title,Description,Slug,Content,Published,CategoryIDs")] CreatPostModel postModel, int[] selectedCategories)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title"  );

            if (string.IsNullOrEmpty(post.Slug))
            {
                post.Slug = AppUtilities.GenerateSlug(post.Title);
                //post.Slug = GenerateSlug(post.Title);
            }

            if (!string.IsNullOrEmpty(postModel.Slug) && await _context.Posts.AnyAsync(p => p.Slug == postModel.Slug))
            {
                ModelState.AddModelError("Slug", "Nhap chuoi url khac");
                return View(postModel);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update post properties from model
                    post.Title = postModel.Title;
                    post.Description = postModel.Description;
                    post.Slug = postModel.Slug;
                    post.Content = postModel.Content;
                    post.Published = postModel.Published;
                    post.DateUpdated = DateTime.Now;

                    if (string.IsNullOrEmpty(post.Slug))
                    {
                        post.Slug = AppUtilities.GenerateSlug(post.Title);
                       // post.Slug = GenerateSlug(post.Title);
                    }

                    var existingPostCategories = await _context.PostCategories
                        .Where(pc => pc.PostID == id)
                        .ToListAsync();

                    _context.PostCategories.RemoveRange(existingPostCategories);

                    // Use CategoryIDs from the model instead of selectedCategories parameter
                    if (postModel.CategoryIDs != null && postModel.CategoryIDs.Length > 0)
                    {
                        foreach (var categoryId in postModel.CategoryIDs)
                        {
                            var postCategory = new PostCategory
                            {
                                PostID = post.PostId,
                                CategoryID = categoryId
                            };
                            _context.Add(postCategory);
                        }
                    }

                    _context.Update(post);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(post.PostId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

           /* var categories = await _context.Categories.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title", selectedCategories);*/
            return View(post);
        }

        // GET: Post/Delete/5
        [HttpGet("/admin/post/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.PostId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost("/admin/post/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            var postCategories = await _context.PostCategories
                .Where(pc => pc.PostID == id)
                .ToListAsync();

            _context.PostCategories.RemoveRange(postCategories);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            StatusMessage = "Ban vua xoa bai viet: " + post.Title;
            
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }

        private string GenerateSlug(string title)
        {
            var slug = title.ToLowerInvariant();
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9-]", "");
            return slug;
        }
    }
}
