using Do_An_Chuyen_Nganh.Data;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/homeadmin")]
    public class HomeAdminController : Controller
    {
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
