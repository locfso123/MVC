using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace mvc01.Models.Contact
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName ="nvarchar")]
        [StringLength(50)]
        [DisplayName("Ho ten")]
        [Required(ErrorMessage ="Phai nhap {0}")]
        public string FullName { get; set; }

        [StringLength(100)]
        [Required(ErrorMessage ="Phai nhap {0}")]
        [EmailAddress(ErrorMessage ="Phai la dia chi Email")]
        [DisplayName("Dia chi Email")]
        public string Email { get; set; }

        public DateTime? DateSent { get; set; }

        [DisplayName("Noi dung")]
        public string? Message { get; set; }

        [StringLength(50)]
        [Phone(ErrorMessage ="Phai la so dien thoai")]
        [DisplayName("So dien thoai")]
        public string? phone {  get; set; }
    }
}
