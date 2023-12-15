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

        [HttpGet("Getstatistical")]
        public ActionResult GetStatistical(string fromDate, string toDate)
        {
            DateTime startDate = string.IsNullOrEmpty(fromDate) ? DateTime.MinValue : DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
            DateTime endDate = string.IsNullOrEmpty(toDate) ? DateTime.MaxValue : DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
            var query = from o in _context.Orders
                        join od in _context.OrderDetails on o.Id equals od.Id
                        join p in _context.Products on od.ProductId equals p.Id
                        where o.CreatedAt.Date >= startDate.Date && o.CreatedAt.Date <= endDate.Date
                        select new
                        {
                            CreatedAt = o.CreatedAt,
                            Quantity = od.Quantity,
                            Price = od.Price,
                            OriginalPrice = p.Price
                        };

            var result = query
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

        [HttpGet("GetOrderStatistics")]
        public ActionResult GetOrderStatistics()
        {
            var orderStatistics = _context.Orders
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalOrders = g.Count()
                })
                .OrderByDescending(x => x.Date)
                .ToList();

            // Calculate the total number of orders (you can customize this based on your specific needs)
            var totalOrders = orderStatistics.Sum(x => x.TotalOrders);

            return Json(new { TotalOrders = totalOrders, OrderStatistics = orderStatistics });
        }
    }
}
