using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Do_An_Chuyen_Nganh.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ManageProduct")]
    public class ManageProductController : BaseController<Product>
    {
        private readonly ApplicationDbContext _context;

        public ManageProductController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        [HttpGet("GetUpdateStatus")] 
        public async Task<ActionResult> GetUpdateStatus()
        {
           
            var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.Color).Include(p => p.Condition).Include(p => p.Provenience).Include(p => p.Warranty);
            return View(await applicationDbContext.ToListAsync());
        }

        [HttpPost("UpdateStatus")]      
        public async Task<IActionResult> UpdateStatus(int productId, string newStatus)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                Enum.TryParse(newStatus, out ProductStatus status);
                product.Status = status;
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

       
    }
}
