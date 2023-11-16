using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Data;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Components
{
    public class HeroSection : ViewComponent
    {
       
        private readonly ApplicationDbContext _context; // Thêm ApplicationDbContext

        public HeroSection(ApplicationDbContext context)
        {

            _context = context; // Inject ApplicationDbContext vào controller
        }
        public IViewComponentResult Invoke()
        {
            return View(_context.Categories.ToList());
        }
    }
}
