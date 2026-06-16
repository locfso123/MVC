using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace mvc._01.MyView
{
    public class xinchao2Model : PageModel
    {
        public string Message { get; set; } = "Xin chao";

        public void OnGet()
        {
        }
    }
}
