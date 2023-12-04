using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Do_An_Chuyen_Nganh.Models.Enums;
using Do_An_Chuyen_Nganh.Data;

namespace Do_An_Chuyen_Nganh.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Code { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không vượt quá 150 ký tự")]
        public string? CustomerName { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Bạn cần chọn phương thức thanh toán")]
        public string? Payment { get; set; }
        public decimal Total { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        public string? Email { get; set; }

        public int Paid { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public ICollection<OrderDetail>? OrderDetails { get; set; }

       

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
    }
}
