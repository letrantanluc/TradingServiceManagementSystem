using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_An_Chuyen_Nganh.Controllers
{
    public class WishListController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WishListController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult WishList()
        {
            // Lấy thông tin người dùng hiện tại (đã đăng nhập)
            string userName = User.Identity.Name;

            // Lấy danh sách yêu thích của người dùng
            var wishList = _context.WishList
                .Include(w => w.Product)
                .Where(w => w.UserName == userName).Include(p => p.Product.Images)
                .ToList();

            return View(wishList);
        }
      
        [HttpPost]
        public IActionResult AddToWishList(int productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Người dùng đã đăng nhập
                string userName = User.Identity.Name;

                // Kiểm tra xem sản phẩm đã tồn tại trong danh sách yêu thích chưa
                var existingWishListItem = _context.WishList
                    .FirstOrDefault(w => w.ProductId == productId && w.UserName == userName);

                if (existingWishListItem == null)
                {
                    // Nếu sản phẩm chưa tồn tại trong danh sách yêu thích, thêm mới
                    var newWishListItem = new WishList
                    {
                        ProductId = productId,
                        UserName = userName,
                        CreatedDate = DateTime.Now
                    };

                    _context.WishList.Add(newWishListItem);
                    _context.SaveChanges();

                    // Trả về kết quả thành công (hoặc có thể trả về thông tin khác nếu cần)
                    return Json(new { success = true, message = "Đã thêm vào danh sách yêu thích!" });
                }

                // Trả về kết quả thông báo nếu sản phẩm đã tồn tại trong danh sách yêu thích
                return Json(new { success = false, message = "Sản phẩm đã có trong danh sách yêu thích!" });
            }

            // Trả về kết quả thông báo nếu người dùng chưa đăng nhập
            return Json(new { success = false, message = "Vui lòng đăng nhập để thêm vào danh sách yêu thích." });
        }

        [HttpPost]
        public IActionResult RemoveFromWishList(int productId)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userName = User.Identity.Name;

                // Tìm sản phẩm trong danh sách yêu thích của người dùng
                var wishListItem = _context.WishList
                    .FirstOrDefault(w => w.ProductId == productId && w.UserName == userName);

                if (wishListItem != null)
                {
                    // Xóa sản phẩm khỏi danh sách yêu thích
                    _context.WishList.Remove(wishListItem);
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Đã xóa khỏi danh sách yêu thích." });
                }
            }

            return Json(new { success = false, message = "Không thể xóa sản phẩm khỏi danh sách yêu thích." });
        }


    }
}
