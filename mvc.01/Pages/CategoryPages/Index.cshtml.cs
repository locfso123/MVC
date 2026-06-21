using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mvc01.Models.Blog;
using mvc01.Models;

namespace mvc01.Pages.CategoryPages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

    public IndexModel(AppDbContext context)
    {
        _context = context;
    }

    public IList<Category> Category { get; set; } = default!;

    public async Task OnGetAsync()
    {
        Category = await _context.Categories.ToListAsync();
    }
    }
}
