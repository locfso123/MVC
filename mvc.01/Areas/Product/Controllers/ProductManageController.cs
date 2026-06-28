using App.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc01.Areas.Product.Models;


//using mvc01.Areas.Product.Models;
//using mvc01.Areas.Product.Models
using mvc01.Data;
using mvc01.Models;
using mvc01.Models.Blog;
using mvc01.Models.Product;

namespace mvc01.Areas.Product.Controllers
{
    [Area("Product")]
    [Route("admin/productmanage/[action]/{id?}")]   
    [Authorize(Roles = RoleName.Administrator + "," + RoleName.Editor)]
    public class ProductManageController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductManageController(AppDbContext dbContext, UserManager<AppUser> userManager)
        {
            _context = dbContext;
            _userManager = userManager;
        }

        [TempData]
        public string StatusMessage { get; set; }

        // GET: Post
        // [HttpGet("/admin/post")]
        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name= "p")] int currentPage ,int pagesize)
        {
            var posts = _context.Products
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
                        .Include(p => p.ProductCategoryProducts)
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
       // [HttpGet("/admin/post/detail/{id}")]
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Products
                .Include(p => p.Author)
                .Include(p => p.ProductCategoryProducts)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Post/Create
       // [HttpGet("/admin/post/create")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title");
            return View();
        }

        // POST: Post/Create
       // [HttpPost("/admin/post/create")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title,Description,Slug,Content,Published,CategoryIDs,Price")]  CreateProductModel product, int[] selectedCategories)
        {
            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title", selectedCategories);

            if (ModelState.IsValid)
            {
                /*if (!string.IsNullOrEmpty(postModel.Slug) && await _context.Products.AnyAsync(p => p.Slug == postModel.Slug))
                {
                    ModelState.AddModelError("Slug", "Nhap chuoi url khac");
                    return View(postModel);
                }*/

                var user = await _userManager.GetUserAsync(this.User);
                if (user == null)
                {
                    return RedirectToAction("Index", "Home");   
                }

                // Map CreateProductModel to ProductModel
                var newProduct = new ProductModel
                {
                    Title = product.Title,
                    Description = product.Description,
                    Slug = product.Slug,
                    Content = product.Content,
                    Published = product.Published,
                    DateCreated = DateTime.Now,
                    DateUpdated = DateTime.Now,
                    AuthorId = user.Id,
                    Price = product.Price
                };

                if (string.IsNullOrEmpty(newProduct.Slug))
                {
                    newProduct.Slug = AppUtilities.GenerateSlug(newProduct.Title);
                }

                if (!string.IsNullOrEmpty(product.Slug) && await _context.Products.AnyAsync(p => p.Slug == product.Slug))
                {
                    ModelState.AddModelError("Slug", "Nhap chuoi url khac");
                    return View(product);
                }

                _context.Add(newProduct);
                await _context.SaveChangesAsync();

                // Use CategoryIDs from the model instead of selectedCategories parameter
                if (product.CategoryIDs != null && product.CategoryIDs.Length > 0)
                {
                    foreach (var CateId in product.CategoryIDs)
                    {
                        _context.Add(new ProductCategoryProduct()
                        {
                            CategoryID = CateId,
                            ProductID = newProduct.ProductId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                StatusMessage = "Vua tao xong bai viet moi";

                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        // GET: Post/Edit/5
        // [HttpGet("/admin/post/edit/{id}")]
        [HttpGet]
         public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductCategoryProducts)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            var postEdit = new CreateProductModel()
            {
                ProductId = product.ProductId,
                Title = product.Title,
                Content = product.Content,
                Description = product.Description,
                Slug = product.Slug,
                Published = product.Published,
                CategoryIDs = product.ProductCategoryProducts.Select(pc => pc.CategoryID).ToArray(),
                Price = product.Price
            };

            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title", postEdit.CategoryIDs);

            return View(postEdit);
        }

        // POST: Post/Edit/5
       // [HttpPost("/admin/post/edit/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Title,Description,Slug,Content,Published,CategoryIDs,Price")] CreateProductModel product, int[] selectedCategories)
        {
            var productUpdate = await _context.Products.FindAsync(id);
            if (productUpdate == null)
            {
                return NotFound();
            }

            var categories = await _context.CategoryProducts.ToListAsync();
            ViewData["Categories"] = new MultiSelectList(categories, "Id", "Title");

            if (string.IsNullOrEmpty(productUpdate.Slug))
            {
                productUpdate.Slug = AppUtilities.GenerateSlug(productUpdate.Title);
                //post.Slug = GenerateSlug(post.Title);
            }

            if (!string.IsNullOrEmpty(product.Slug) && await _context.Products.AnyAsync(p => p.Slug == product.Slug))
            {
                ModelState.AddModelError("Slug", "Nhap chuoi url khac");
                return View(product);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update post properties from model
                    productUpdate.Title = product.Title;
                    productUpdate.Description = product.Description;
                    productUpdate.Slug = product.Slug;
                    productUpdate.Content = product.Content;
                    productUpdate.Published = product.Published;
                    productUpdate.DateUpdated = DateTime.Now;
                    productUpdate.Price = product.Price;

                    if (string.IsNullOrEmpty(productUpdate.Slug))
                    {
                        productUpdate.Slug = AppUtilities.GenerateSlug(productUpdate.Title);
                       // post.Slug = GenerateSlug(post.Title);
                    }

                    var existingProductCategories = await _context.ProductCategoryProducts
                        .Where(pc => pc.ProductID == id)
                        .ToListAsync();

                    _context.ProductCategoryProducts.RemoveRange(existingProductCategories);

                    // Use CategoryIDs from the model instead of selectedCategories parameter
                    if (product.CategoryIDs != null && product.CategoryIDs.Length > 0)
                    {
                        foreach (var categoryId in product.CategoryIDs)
                        {
                            var productCategory = new ProductCategoryProduct
                            {
                                ProductID = productUpdate.ProductId,
                                CategoryID = categoryId
                            };
                            _context.Add(productCategory);
                        }
                    }

                    _context.Update(productUpdate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(productUpdate.ProductId))
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
            return View(productUpdate);
        }

        // GET: Post/Delete/5
       // [HttpGet("/admin/post/delete/{id}")]
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Author)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Post/Delete/5
       // [HttpPost("/admin/post/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            var productCategories = await _context.ProductCategoryProducts
                .Where(pc => pc.ProductID == id)
                .ToListAsync();

            _context.ProductCategoryProducts.RemoveRange(productCategories);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            StatusMessage = "Ban vua xoa san pham: " + product.Title;
            
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
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
