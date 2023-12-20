using Do_An_Chuyen_Nganh.Controllers;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    [Route("Admin/ManageCondition")]
    public class ManageConditionController : BaseController<Condition>
    {
        private readonly ApplicationDbContext _context;

        public ManageConditionController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet("Index")] // Chỉ rõ là phương thức GET
        public async Task<ActionResult> Index()
        {
            var conditions = GetAll().ToList();
            return View(conditions);
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(IFormCollection formCollection, Condition condition)
        {
            List<string> errors = new List<string>();
            try
            {
                var name = formCollection["ConditionName"].ToString();
                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên tình trạng");
                }
                if (errors.Count == 0)
                {
                    Add(condition);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;

            return RedirectToAction("Index", "ManageCondition");
        }

        [Route("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index", "ManageCondition");
            }

            var checkId = GetById(Id.Value);

            if (checkId == null)
            {
                return RedirectToAction("Index", "ManageCondition");
            }

            Condition condition = GetById(Id.Value);
            ViewBag.Conditions = GetAll().ToList();
            return View(checkId);
        }

        [HttpPost("Edit/{id}")]
        public async Task<ActionResult> Edit(IFormCollection formCollection)
        {
            List<string> errors = new List<string>();
            try
            {
                var id = formCollection["Id"].ToString();
                var name = formCollection["ConditionName"].ToString();

                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên tình trạng");
                }
                if (errors.Count == 0)
                {
                    var condition = GetById(Int32.Parse(formCollection["Id"]));
                    condition.ConditionName = name;
                    Update(condition);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;
            return RedirectToAction("Index", "ManageCondition");
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
                Condition condition = GetById(id.Value);

                if (condition == null)
                {
                    return Json(new { success = false, message = "Tình Trạng không tồn tại" });
                }

                // Assuming that your Remove method works correctly
                Remove(condition);

                return Json(new { success = true, message = "Xóa tình trạng thành công" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Lỗi khi xóa tình trạng" });
            }
        }
    }
}
