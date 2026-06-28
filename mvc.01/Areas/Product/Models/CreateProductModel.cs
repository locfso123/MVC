using mvc01.Models.Blog;
using mvc01.Models.Product;
using System.ComponentModel.DataAnnotations;

namespace mvc01.Areas.Product.Models
{
    public class CreateProductModel : ProductModel
    {
        /*[Required(ErrorMessage = "Phải có tiêu đề bài viết")]
        [Display(Name = "Tiêu đề")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        public string Title { set; get; }

        [Display(Name = "Mô tả ngắn")]
        public string? Description { set; get; }

        [Display(Name = "Chuỗi định danh (url)", Prompt = "Nhập hoặc để trống tự phát sinh theo Title")]
        [StringLength(160, MinimumLength = 5, ErrorMessage = "{0} dài {1} đến {2}")]
        [RegularExpression(@"^[a-z0-9-]*$", ErrorMessage = "Chỉ dùng các ký tự [a-z0-9-]")]
        public string? Slug { set; get; }

        [Display(Name = "Nội dung")]
        public string? Content { set; get; }

        [Display(Name = "Xuất bản")]
        public bool Published { set; get; }*/

        [Display(Name = "Chuyen muc")]
        public int[] CategoryIDs { get; set; }
    }
}
