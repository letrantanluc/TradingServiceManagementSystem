using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using X.PagedList;
using Do_An_Chuyen_Nganh.Models.ViewModels;

namespace Do_An_Chuyen_Nganh.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public int PageSize = 9;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //ProductInCateogry này là vừa ở HeroSection click vào category nào nó hiện sp category đó
        // Nhưng phần phân trang xấu nên phải dùng viewmodel chỉnh lại
        //categoryId là gán cho nó bít Id của category khi phân trang, CategoryId là truyền Category từ HeroSection qua
        public async Task<ActionResult> ProductInCategory(int categoryId, int CategoryId, int productPage = 1)
        {
            return View(
                new ProductListViewModel
                {
                    categoryId = categoryId,
                    Products = await _context.Products
                        .Where(x => x.CategoryId == CategoryId)
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToListAsync(),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = productPage,
                        TotalItems = await _context.Products
                            .Where(x => x.CategoryId == CategoryId)
                            .CountAsync()
                    }
                }
            );
        }

        [HttpPost]
        public async Task<ActionResult> Search(string keywords, int productPage = 1)
        {
            return View("ProductInCategory",
                new ProductListViewModel
                {
                   
                    Products = await _context.Products
                        .Where(x => x.Title.Contains(keywords))
                        .Skip((productPage - 1) * PageSize)
                        .Take(PageSize)
                        .ToListAsync(),
                    PagingInfo = new PagingInfo
                    {
                        ItemsPerPage = PageSize,
                        CurrentPage = productPage,
                        TotalItems = await _context.Products.Where(x => x.Title.Contains(keywords))
                            .CountAsync()
                    }
                }
            );
        }
        /**
         * ProductInCategory khi phân trang bth
           public async Task<IActionResult> ProductInCategory(int CategoryId, int? page)
        {
            var pageSize = 4;
            if (page == null)
            {
                page = 1;
            }

            var items = await _context.Products
                .Where(x => x.CategoryId == CategoryId)
                .OrderBy(x => x.Title)
                .ToListAsync();

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            var a = items.ToPagedList(pageIndex, pageSize);

            if (items.Any())
            {
                ViewBag.PageSize = pageSize;
                ViewBag.Page = page;
                ViewBag.CategoryId = CategoryId;
                return View(a);
            }
            else
            {
                ViewBag.Message = "Không có sản phẩm thuộc danh mục này";
                return View();
            }
        }

        **/




        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.Color).Include(p => p.Condition).Include(p => p.Provenience).Include(p => p.Warranty);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Condition)
                .Include(p => p.Provenience)
                .Include(p => p.Warranty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName");
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "Id", "ConditionName");
            ViewData["ProvenienceId"] = new SelectList(_context.Proveniences, "Id", "ProvenienceName");
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "WarrantyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,image,CategoryId,ColorId,ConditionId,ProvenienceId,WarrantyId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", product.ColorId);
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "Id", "ConditionName", product.ConditionId);
            ViewData["ProvenienceId"] = new SelectList(_context.Proveniences, "Id", "ProvenienceName", product.ProvenienceId);
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "WarrantyName", product.WarrantyId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", product.ColorId);
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "Id", "ConditionName", product.ConditionId);
            ViewData["ProvenienceId"] = new SelectList(_context.Proveniences, "Id", "ProvenienceName", product.ProvenienceId);
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "WarrantyName", product.WarrantyId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,image,CategoryId,ColorId,ConditionId,ProvenienceId,WarrantyId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", product.ColorId);
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "Id", "ConditionName", product.ConditionId);
            ViewData["ProvenienceId"] = new SelectList(_context.Proveniences, "Id", "ProvenienceName", product.ProvenienceId);
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "WarrantyName", product.WarrantyId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Color)
                .Include(p => p.Condition)
                .Include(p => p.Provenience)
                .Include(p => p.Warranty)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
