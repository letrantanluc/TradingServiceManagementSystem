using Do_An_Chuyen_Nganh.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Models;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ManageStatistical")]
    public class ManageStatisticalController : BaseController<Statistical>
    {
        private readonly ApplicationDbContext _context;

        public ManageStatisticalController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public async Task<ActionResult> Index()
        {
            return View();
        }

        [HttpGet("getstatistical")]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            var querry = from o in _context.Orders
                         join od in _context.OrderDetails on o.Id equals od.Id
                         join p in _context.Products on od.ProductId equals p.Id
                         select new
                         {
                             CreatedAt = o.CreatedAt,
                             Quantity = od.Quantity,
                             Price = od.Price,
                             OriginalPrice = p.Price
                         };
            if (!string.IsNullOrEmpty(fromDate))
            {
                DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                querry = querry.Where(x => x.CreatedAt == startDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                querry = querry.Where(X => X.CreatedAt == endDate);
            }
            var result = querry
               .GroupBy(x => x.CreatedAt.Date)
               .Select(x => new
               {
                   Date = x.Key,
                   TotalBuy = x.Sum(y => y.Quantity * y.OriginalPrice),
                   TotalSell = x.Sum(y => y.Quantity * y.Price),
               })
               .Select(x => new
               {
                   Date = x.Date,
                   DoanhThu = x.TotalSell,
                   LoiNhuan = x.TotalSell - x.TotalBuy,
               });
            return Json(new { Date = result });
        }
    }
}
