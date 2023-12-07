using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Do_An_Chuyen_Nganh.Data;

namespace Do_An_Chuyen_Nganh.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(500, ErrorMessage = "Không vượt quá 500 ký tự")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Mô tả không được để trống")]
        [StringLength(500, ErrorMessage = "Không vượt quá 500 ký tự")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        public decimal Price { get; set; }



        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }


        [Display(Name = "Color")]
        public int ColorId { get; set; }
        public Color? Color { get; set; }

        [Display(Name = "Condition")]
        public int ConditionId { get; set; }
        public Condition? Condition { get; set; }

        [Display(Name = "Provenience")]
        public int ProvenienceId { get; set; }
        public Provenience? Provenience { get; set; }

        [Required(ErrorMessage = "Số lượng  không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng sản phẩm phải lớn hơn 0.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Địa chỉ không được để trống")]
        public string? Address { get; set; }


        [Display(Name = "Warranty")]
        public int WarrantyId { get; set; }
        public Warranty? Warranty { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser? User { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<WishList>? WishLists { get; set; }


        [NotMapped]
        public List<IFormFile>? ImageFile { get; set; }

        public virtual ICollection<ProductImage>? Images { get; set; }


    }

}
