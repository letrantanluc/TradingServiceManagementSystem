using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Microsoft.AspNetCore.Authentication;
using Do_An_Chuyen_Nganh.Controllers;

namespace Do_An_Chuyen_Nganh.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/ManageCategory")]
    public class ManageCategoryController : BaseController<Category>
    {
        private readonly ApplicationDbContext _context;

        public ManageCategoryController(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        [HttpGet("Index")] // Chỉ rõ là phương thức GET
        public async Task<ActionResult> Index()
        {
            var categories = GetAll().ToList();
            return View(categories);
        }

        [HttpGet("Create")]
        public async Task<ActionResult> Create()
        {
            return View();
        }
        [HttpPost("Create")]
        public async Task<ActionResult> Create(IFormCollection formCollection, Category category)
        {
            List<string> errors = new List<string>();
            try
            {
                var name = formCollection["CategoryName"].ToString();
                var slug = formCollection["slug"].ToString();
                var parentid = formCollection["ParentId"].ToString();
                var checkSlug = _context.Categories.Count(x => x.Slug == slug);
                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên danh mục");
                }
                if (checkSlug > 0)
                {
                    errors.Add("Slug đã tồn tại");
                }
                if (errors.Count == 0)
                {
                    Add(category);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;

            return RedirectToAction("Index", "ManageCategory");
        }
        [Route("Edit")]
        [HttpGet("Edit/{id}")]
        public async Task<ActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return RedirectToAction("Index", "ManageCategory");
            }

            var checkId = GetById(Id.Value);

            if (checkId == null)
            {
                return RedirectToAction("Index", "ManageCategory");
            }

            Category category = GetById(Id.Value);
            ViewBag.Categories = GetAll().ToList();
            return View(checkId);
        }

        [HttpPost("Edit/{id}")]
        public async Task<ActionResult> Edit(IFormCollection formCollection)
        {
            List<string> errors = new List<string>();
            try
            {
                var id = formCollection["Id"].ToString();
                var name = formCollection["CategoryName"].ToString();
                var slug = formCollection["Slug"].ToString();
                var parentId = formCollection["ParentId"];

                var checkSlug = _context.Categories.Count(x => x.Slug == slug);
                var getCategoryContainsSlug = _context.Categories.FirstOrDefault(x => x.Slug == slug);
                if (string.IsNullOrEmpty(name))
                {
                    errors.Add("Chưa nhập tên danh mục");
                }
                if (checkSlug > 0 && getCategoryContainsSlug.Id != Convert.ToInt32(id))
                {
                    errors.Add("Slug đã tồn tại");
                }
                if (errors.Count == 0)
                {
                    var category = GetById(Int32.Parse(formCollection["Id"]));
                    category.CategoryName = name;
                    category.Slug = slug;
                    category.ParentId = Int32.Parse(parentId);
                    Update(category);
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            TempData["Errors"] = errors;
            return RedirectToAction("Index", "ManageCategory");
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
                Category category = GetById(id.Value);

                if (category == null)
                {
                    return Json(new { success = false, message = "Danh mục không tồn tại" });
                }

                // Assuming that your Remove method works correctly
                Remove(category);

                return Json(new { success = true, message = "Xóa danh mục thành công" });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging purposes
                Console.WriteLine(ex.Message);
                return Json(new { success = false, message = "Lỗi khi xóa danh mục" });
            }
        }

    }
}