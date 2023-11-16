using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Controllers
{
    public class WishListController : Controller
    {
        private readonly ApplicationDbContext _context;
        public WishListController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostWishList(int ProductId)
        {
            var item = new WishList();
            item.ProductId = ProductId;
            item.UserName = User.Identity.Name;
            item.CreatedDate= DateTime.Now;
            _context.WishList.Add(item);
            _context.SaveChanges();
            return Json(new {Success = true});
        }
    }
}
