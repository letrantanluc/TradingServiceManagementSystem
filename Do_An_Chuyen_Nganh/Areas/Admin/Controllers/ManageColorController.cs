using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/ManageColor")]
    public class ManageColorController : BaseController<Color>
    {
        private readonly ApplicationDbContext _context;

        public ManageColorController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet("Index")] // Chỉ rõ là phương thức GET
        public async Task<ActionResult> Index()
        {
            var colors = GetAll().ToList();
            return View(colors);
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(IFormCollection formCollection, Color color)
        {
            List<string> errors = new List<string>();
            try
            {
                var name = formCollection["ColorName"].ToString();
                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên màu");
                }
                if (errors.Count == 0)
                {
                    Add(color);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;

            return RedirectToAction("Index", "ManageColor");
        }

        [Route("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index", "ManageColor");
            }

            var checkId = GetById(Id.Value);

            if (checkId == null)
            {
                return RedirectToAction("Index", "ManageColor");
            }

            Color color = GetById(Id.Value);
            ViewBag.Colors = GetAll().ToList();
            return View(checkId);
        }

        [HttpPost("Edit/{id}")]
        public async Task<ActionResult> Edit(IFormCollection formCollection)
        {
            List<string> errors = new List<string>();
            try
            {
                var id = formCollection["Id"].ToString();
                var name = formCollection["ColorName"].ToString();
              
                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên màu");
                }
                if (errors.Count == 0)
                {
                    var color = GetById(Int32.Parse(formCollection["Id"]));
                    color.ColorName = name;
                    Update(color);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;
            return RedirectToAction("Index", "ManageColor");
        }

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return Json(new { success = false, message = "ID không hợp lệ" });
            }

            try
            {
                Color color = GetById(id.Value);

                if (color == null)
                {
                    return Json(new { success = false, message = "Màu không tồn tại" });
                }

                // Assuming that your Remove method works correctly
                Remove(color);

                return Json(new { success = true, message = "Xóa màu thành công" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Lỗi khi xóa màu" });
            }
        }
    }
}
