using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Do_An_Chuyen_Nganh.Service;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Controllers
{
    public class MessageController : BaseController<Message>
    {
        private readonly ApplicationDbContext _context;

        public MessageController(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _context = context;

        }
        public async Task<ActionResult> Index()
        {

            return View();
        }
    }
}
