using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Do_An_Chuyen_Nganh.Service;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CartManager _cartManager;

        public CartController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _cartManager = new CartManager(httpContextAccessor);
        }

        public ActionResult Index()
        {
            var cart = _cartManager.GetCartItems();
            ViewBag.TongSoLuongSanPham = TongSoLuongSanPham();
            if (cart.Count() < 1)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(cart);
        }


        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product == null)
            {
                return Json(new { status = false, message = "Sản phẩm không tồn tại" });
            }
            if (quantity <= 0)
            {
                return Json(new { status = false, message = "Số lượng không hợp lệ" });
            }
            var item = new CartItem
            {
                ProductId = productId,
                Quantity = quantity,
                ProductName = product.Title,
                Price = product.Price,
            };
            _cartManager.AddToCart(item);
            return Json(new { status = true, message = "Thêm vào giỏ hàng thành công" });
        }

        [HttpPost]
        public JsonResult UpdateCart(Guid itemId, int quantity)
        {
            var cart = _cartManager.GetCartItems();
            var item = cart.FirstOrDefault(x => x.Id == itemId);
            var product = _context.Products.FirstOrDefault(p => p.Id == item.ProductId);
            if (item == null)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại trong giỏ hàng" });
            }
            if (product.Quantity < quantity)
            {
                item.Quantity = product.Quantity;
                _cartManager.UpdateCart(cart);
                return Json(new { success = false, message = "Sản phẩm không có đủ số lượng" });
            }
            item.Quantity = quantity;

            _cartManager.UpdateCart(cart);

            return Json(new { success = true, message = "Cập nhật giỏ hàng thành công" });
        }

        [HttpGet]
        public JsonResult GetTotal()
        {
            int total = 0;
            var cart = _cartManager.GetCartItems();
            foreach (var item in cart)
            {
                total += item.Quantity;
            }
            return Json(new { success = true, total = total });
        }

        [HttpPost]
        public ActionResult RemoveFromCart(Guid cartItemId)
        {
            _cartManager.RemoveFromCart(cartItemId);

            return Json(new { success = true });
        }

        [HttpPost]
        public ActionResult ClearCart()
        {
            _cartManager.ClearCart();

            return Json(new { success = true });
        }
        private int TongSoLuongSanPham()
        {
            int totalQuantity = 0;
            var cartItems = _cartManager.GetCartItems();
            if (cartItems != null)
            {
                totalQuantity = cartItems.Sum(item => item.Quantity);
            }
            return totalQuantity;
        }
    }
}
