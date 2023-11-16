using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Chuyen_Nganh.Models
{
    public class WishList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string? UserName { get; set; }

        public DateTime CreatedDate { get; set; }

        public Product? Product { get; set; }


    }
}
