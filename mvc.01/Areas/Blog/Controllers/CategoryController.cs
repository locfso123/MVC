using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mvc01.Data;
using mvc01.Models;
using mvc01.Models.Blog;

namespace mvc01.Areas.Blog.Controllers
{
    [Area("Blog")]
    [Route("admin/blog/category/[action]/{id?}")]
    [Authorize(Roles= RoleName.Administrator)]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext dbContext)
        {
            _context = dbContext;
        }

        // GET: Category
        [HttpGet("/admin/category")]
        public async Task<IActionResult> Index()
        {
            var qr = (from c in _context.Categories select c)
                      .Include(c=> c.ParentCategory)
                      .Include(c=>c.CategoryChildren);

            var Categories = (await qr.ToListAsync())
                           .Where(c => c.ParentCategory == null)
                           .ToList();

            return View(Categories);
            //return View(await _context.Categories.ToListAsync());
        }

        // GET: Category/Details/5
        [HttpGet("/admin/category/detail/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.CategoryChildren)
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        private void CreateSelectItems(List<Category> source, List<Category> des, int level)
        {
            string prefix = string.Concat(Enumerable.Repeat("----", level));
            foreach (var category in source)
            {
                // category.Title = prefix + " " + category.Title;
                des.Add(new Category()
                {
                    Id = category.Id,
                    Title = prefix + " " + category.Title
                });
                if (category.CategoryChildren?.Count > 0)
                {
                    CreateSelectItems(category.CategoryChildren.ToList(), des, level + 1);
                }
            }
        }

        // GET: Category/Create
        [HttpGet("/admin/category/create")]
        public async Task<IActionResult> CreateAsync()
        {
            var qr = (from c in _context.Categories select c)
                      .Include(c => c.ParentCategory)
                      .Include(c => c.CategoryChildren);

            var Categories = (await qr.ToListAsync())
                           .Where(c => c.ParentCategory == null)
                           .ToList();

            Categories.Insert(0, new Category 
            { 
                Id = -1, 
                Title = "Khong co danh muc cha" 
            });

            var items = new List<Category>();
            CreateSelectItems(Categories, items, 0);    

            var selectList = new SelectList(items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;
            return View();
        }

        // POST: Category/Create
        [HttpPost("/admin/category/create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Slug,ParentCategoryId")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ParentCategoryId == -1)
                {
                    category.ParentCategoryId = null;
                }

                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var qr = (from c in _context.Categories select c)
                      .Include(c => c.ParentCategory)
                      .Include(c => c.CategoryChildren);

            var Categories = (await qr.ToListAsync())
                           .Where(c => c.ParentCategory == null)
                           .ToList();

            Categories.Insert(0, new Category
            {
                Id = -1,
                Title = "Khong co danh muc cha"
            });

            var items = new List<Category>();
            CreateSelectItems(Categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;

            return View(category);
        }

        // GET: Category/Edit/5
        [HttpGet("/admin/category/edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var qr = (from c in _context.Categories select c)
                    .Include(c => c.ParentCategory)
                    .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                             .Where(c => c.ParentCategory == null)
                             .ToList();
            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Category>();
            CreateSelectItems(categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost("/admin/category/edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Slug,ParentCategoryId")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            bool canUpdate = true;

            if (category.ParentCategoryId == category.Id)
            {
                ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khác");
                canUpdate = false;
            }

            // Kiem tra thiet lap muc cha phu hop
            if (canUpdate && category.ParentCategoryId != null)
            {
                var childCates =
                            (from c in _context.Categories select c)
                            .Include(c => c.CategoryChildren)
                            .ToList()
                            .Where(c => c.ParentCategoryId == category.Id);


                // Func check Id 
                Func<List<Category>, bool> checkCateIds = null;
                checkCateIds = (cates) =>
                {
                    foreach (var cate in cates)
                    {
                        Console.WriteLine(cate.Title);
                        if (cate.Id == category.ParentCategoryId)
                        {
                            canUpdate = false;
                            ModelState.AddModelError(string.Empty, "Phải chọn danh mục cha khácXX");
                            return true;
                        }
                        if (cate.CategoryChildren != null)
                            return checkCateIds(cate.CategoryChildren.ToList());

                    }
                    return false;
                };
                // End Func 
                checkCateIds(childCates.ToList());
            }




            if (ModelState.IsValid && canUpdate)
            {
                try
                {
                    if (category.ParentCategoryId == -1)
                        category.ParentCategoryId = null;

                    var dtc = _context.Categories.FirstOrDefault(c => c.Id == id);
                    _context.Entry(dtc).State = EntityState.Detached;

                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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

            var qr = (from c in _context.Categories select c)
                     .Include(c => c.ParentCategory)
                     .Include(c => c.CategoryChildren);

            var categories = (await qr.ToListAsync())
                             .Where(c => c.ParentCategory == null)
                             .ToList();

            categories.Insert(0, new Category()
            {
                Id = -1,
                Title = "Không có danh mục cha"
            });
            var items = new List<Category>();
            CreateSelectItems(categories, items, 0);
            var selectList = new SelectList(items, "Id", "Title");

            ViewData["ParentCategoryId"] = selectList;


            return View(category);

            /*if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);*/
        }

        // GET: Category/Delete/5
        [HttpGet("/admin/category/delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost("/admin/category/delete/{id}"), ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                                .Include(c=>c.CategoryChildren)
                                .FirstOrDefaultAsync(c=>c.Id == id);
            if(category == null)
            {
                return NotFound();
            }

            foreach (var cCategory in category.CategoryChildren)
            {
                cCategory.ParentCategoryId = category.ParentCategoryId;
            }

            if (category != null)
            {
                _context.Categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
