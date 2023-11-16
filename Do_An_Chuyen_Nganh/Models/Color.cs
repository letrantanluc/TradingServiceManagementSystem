using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Do_An_Chuyen_Nganh.Models
{
    public class Color
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(150, ErrorMessage = "Không vượt quá 150 ký tự")]
        public string? ColorName { get; set; }

        public ICollection<Product>? Products { get; set; }
    }
}
