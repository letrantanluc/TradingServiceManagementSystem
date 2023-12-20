using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/ManageProvenience")]
    public class ManageProvenienceController : BaseController<Provenience>
    {
        private readonly ApplicationDbContext _context;

        public ManageProvenienceController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet("Index")] // Chỉ rõ là phương thức GET
        public async Task<ActionResult> Index()
        {
            var proveniences = GetAll().ToList();
            return View(proveniences);
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(IFormCollection formCollection, Provenience provenience)
        {
            List<string> errors = new List<string>();
            try
            {
                var name = formCollection["ProvenienceName"].ToString();
                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên nguồn gốc");
                }
                if (errors.Count == 0)
                {
                    Add(provenience);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;

            return RedirectToAction("Index", "ManageProvenience");
        }

        [Route("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index", "ManageProvenience");
            }

            var checkId = GetById(Id.Value);

            if (checkId == null)
            {
                return RedirectToAction("Index", "ManageProvenience");
            }

            Provenience provenience = GetById(Id.Value);
            ViewBag.Proveniences = GetAll().ToList();
            return View(checkId);
        }

        [HttpPost("Edit/{id}")]
        public async Task<ActionResult> Edit(IFormCollection formCollection)
        {
            List<string> errors = new List<string>();
            try
            {
                var id = formCollection["Id"].ToString();
                var name = formCollection["ProvenienceName"].ToString();

                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên nguồn gốc");
                }
                if (errors.Count == 0)
                {
                    var provenience = GetById(Int32.Parse(formCollection["Id"]));
                    provenience.ProvenienceName = name;
                    Update(provenience);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;
            return RedirectToAction("Index", "ManageProvenience");
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
                Provenience provenience = GetById(id.Value);

                if (provenience == null)
                {
                    return Json(new { success = false, message = "Nguồn gốc không tồn tại" });
                }

                // Assuming that your Remove method works correctly
                Remove(provenience);

                return Json(new { success = true, message = "Xóa nguồn gốc thành công" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Lỗi khi xóa nguồn gốc" });
            }
        }
    }
}
