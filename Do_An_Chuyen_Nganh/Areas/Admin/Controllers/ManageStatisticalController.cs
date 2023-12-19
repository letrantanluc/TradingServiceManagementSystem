using Do_An_Chuyen_Nganh.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Authorization;
using Do_An_Chuyen_Nganh.Models.Enums;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
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
                   LoiNhuan = x.TotalSell,
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

        [HttpGet("GetUserStatistics")]
        public ActionResult GetUserStatistics()
        {
            var userStatistics = _context.Users;
            var totalUsers = _context.Users.Count(); // Assuming you have a Users table
            return Json(new { TotalUsers = totalUsers , UserStatistic = userStatistics});
        }

        [HttpGet("GetTotalProductsSold")]
        public ActionResult GetTotalProductsSold()
        {
            var productStatistics = _context.Products;
            var totalProductsSold = _context.OrderDetails
                                            .Sum(od => od.Quantity); // Giả định rằng mỗi mục trong OrderDetails chứa số lượng sản phẩm được bán

            return Json(new { TotalProductsSold = totalProductsSold, ProductStatistic = productStatistics });
        }

        [HttpGet("GetTotalRevenue")]
        public ActionResult GetTotalRevenue()
        {
            var totalRevenue = _context.OrderDetails
                                       .Sum(od => od.Total); // Tính tổng doanh thu từ trường Total của OrderDetail

            return Json(new { TotalRevenue = totalRevenue });
        }

        [HttpGet("GetProductStatistics")]
        public ActionResult GetProductStatistics()
        {
            // Thực hiện truy vấn để lấy dữ liệu thống kê sản phẩm
            var productStatistics = _context.OrderDetails
                .GroupBy(od => od.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    Quantity = g.Sum(od => od.Quantity)
                })
                .OrderByDescending(x => x.Quantity)
                .Take(10) // Lấy 10 sản phẩm được mua nhiều nhất
                .ToList();

            // Lấy thông tin chi tiết của các sản phẩm từ cơ sở dữ liệu (tên sản phẩm, v.v.)
            var productDetails = _context.Products
                .Where(p => productStatistics.Select(ps => ps.ProductId).Contains(p.Id))
                .Select(p => new
                {
                    ProductId = p.Id,
                    ProductName = p.Title
                })
                .ToList();

            // Kết hợp thông tin thống kê sản phẩm và thông tin chi tiết sản phẩm
            var result = productStatistics
                .Join(productDetails,
                    ps => ps.ProductId,
                    pd => pd.ProductId,
                    (ps, pd) => new
                    {
                        ProductName = pd.ProductName,
                        Quantity = ps.Quantity
                    })
                .ToList();

            return Json(result);
        }

        [HttpGet("GetPeakHourStatistics")]
        public ActionResult GetPeakHourStatistics(string fromDate, string toDate)
        {
            DateTime startDate = string.IsNullOrEmpty(fromDate) ? DateTime.MinValue : DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
            DateTime endDate = string.IsNullOrEmpty(toDate) ? DateTime.MaxValue : DateTime.ParseExact(toDate, "dd/MM/yyyy", null);

            var query = from o in _context.Orders
                        join od in _context.OrderDetails on o.Id equals od.OrderId
                        where o.CreatedAt >= startDate && o.CreatedAt <= endDate
                        group od by new { o.CreatedAt.Date, o.CreatedAt.Hour } into g
                        select new
                        {
                            Date = g.Key.Date,
                            Hour = g.Key.Hour,
                            TotalOrders = g.Count(),
                            TotalRevenue = g.Sum(x => x.Price * x.Quantity)
                        };

            var result = query.OrderBy(x => x.Date).ThenBy(x => x.Hour).ToList();

            return Json(result);
        }

        //public ActionResult ExportExcel()
        //{

        //    return Json(true);
        //}
    }
}
