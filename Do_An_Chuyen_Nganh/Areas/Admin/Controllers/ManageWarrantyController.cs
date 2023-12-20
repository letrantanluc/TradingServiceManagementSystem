using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/ManageWarranty")]
    public class ManageWarrantyController : BaseController<Warranty>
    {
        private readonly ApplicationDbContext _context;

        public ManageWarrantyController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet("Index")] // Chỉ rõ là phương thức GET
        public async Task<ActionResult> Index()
        {
            var warranties = GetAll().ToList();
            return View(warranties);
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(IFormCollection formCollection, Warranty warranty)
        {
            List<string> errors = new List<string>();
            try
            {
                var name = formCollection["WarrantyName"].ToString();
                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên bảo hành");
                }
                if (errors.Count == 0)
                {
                    Add(warranty);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;

            return RedirectToAction("Index", "ManageWarranty");
        }

        [Route("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index", "ManageWarranty");
            }

            var checkId = GetById(Id.Value);

            if (checkId == null)
            {
                return RedirectToAction("Index", "ManageWarranty");
            }

            Warranty warranty = GetById(Id.Value);
            ViewBag.Warranties = GetAll().ToList();
            return View(checkId);
        }

        [HttpPost("Edit/{id}")]
        public async Task<ActionResult> Edit(IFormCollection formCollection)
        {
            List<string> errors = new List<string>();
            try
            {
                var id = formCollection["Id"].ToString();
                var name = formCollection["WarrantyName"].ToString();

                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên bảo hành");
                }
                if (errors.Count == 0)
                {
                    var warranty = GetById(Int32.Parse(formCollection["Id"]));
                    warranty.WarrantyName = name;
                    Update(warranty);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;
            return RedirectToAction("Index", "ManageWarranty");
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
                Warranty warranty = GetById(id.Value);

                if (warranty == null)
                {
                    return Json(new { success = false, message = "Bảo hành không tồn tại" });
                }

                // Assuming that your Remove method works correctly
                Remove(warranty);

                return Json(new { success = true, message = "Xóa bảo hành thành công" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Lỗi khi xóa bảo hành" });
            }
        }



    }
}
