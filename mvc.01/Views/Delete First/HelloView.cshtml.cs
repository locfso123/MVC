using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mvc01.MyView
{
    public class HelloView : PageModel
    {
        public string Message { get; set; } = "Xin chao";

        public void OnGet()
        {
        }
    }
}
