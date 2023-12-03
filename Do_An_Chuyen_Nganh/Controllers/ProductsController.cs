using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Do_An_Chuyen_Nganh.Data;
using Do_An_Chuyen_Nganh.Models;
using Do_An_Chuyen_Nganh.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;


namespace Do_An_Chuyen_Nganh.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;
        public int PageSize = 9;
        public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _context = context;
            _userManager = userManager;
            _env = env;
        }

        // ------------------------------ ProductInCategory ---------------------------------------------------

        //ProductInCateogry này là vừa ở HeroSection click vào category nào nó hiện sp category đó
        // Nhưng phần phân trang xấu nên phải dùng viewmodel chỉnh lại
        //categoryId là gán cho nó bít Id của category khi phân trang, CategoryId là truyền Category từ HeroSection qua
        public async Task<ActionResult> ProductInCategory(int categoryId, int CategoryId, int productPage = 1)
        {

            var products = await _context.Products
                    .Where(x => x.CategoryId == CategoryId)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize)
                    .Select(p => new
                    {
                        Product = p,
                        FirstImage = _context.ProductImages
                                      .Where(img => img.ProductId == p.Id)
                                      .Select(img => img.ImagePath)
                                      .FirstOrDefault()

                    })
                    .ToListAsync();

            var productListViewModel = new ProductListViewModel
            {
                categoryId = categoryId,
                Products = products.Select(p => p.Product).ToList(),
                FirstImages = products.ToDictionary(p => p.Product.Id, p => p.FirstImage),
                PagingInfo = new PagingInfo
                {
                    ItemsPerPage = PageSize,
                    CurrentPage = productPage,
                    TotalItems = await _context.Products
                                                .Where(x => x.CategoryId == CategoryId)
                                                .CountAsync()
                }
            };

            return View(productListViewModel);

            // chưa bỏ ảnh vào, phải làm phức tạp vậy là vì này là productviewmodel chứ ko phải product
            //return View(
            //    new ProductListViewModel
            //    {
            //        categoryId = categoryId,
            //        Products = await _context.Products
            //            .Where(x => x.CategoryId == CategoryId)
            //            .Skip((productPage - 1) * PageSize)
            //            .Take(PageSize)
            //            .ToListAsync(),
            //        PagingInfo = new PagingInfo
            //        {
            //            ItemsPerPage = PageSize,
            //            CurrentPage = productPage,
            //            TotalItems = await _context.Products
            //                .Where(x => x.CategoryId == CategoryId)
            //                .CountAsync()
            //        }
            //    }
            //);
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
            //-------------------------------------------------------------------------------------------
            // --------------------------------------------- Search ---------------------------------------------------

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
        //--------------------------------------------------------------------------------------------------
        // --------------------------------------------- Quản lí tin ---------------------------------------------------
        [Authorize]
        public IActionResult UserPosts()
        {

                // Lấy UserId của người đăng nhập hiện tại
                var userId = _userManager.GetUserId(User);

                // Lấy các bài đăng của người dùng dựa trên UserId
                var userPosts = _context.Products.Where(p => p.UserId == userId).Include(p => p.Images).ToList();

                // Truyền danh sách bài đăng của người dùng đến view
                return View(userPosts);
            }
        //--------------------------------------------------------------------------------------------------
        // --------------------------------------------- Đăng tin ---------------------------------------------------

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName");
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "Id", "ConditionName");
            ViewData["ProvenienceId"] = new SelectList(_context.Proveniences, "Id", "ProvenienceName");
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "WarrantyName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,ImageFile,Address,Quantity,CategoryId,ColorId,ConditionId,ProvenienceId,WarrantyId")] Product product)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                product.UserId = userId;

                // Thêm sản phẩm trước khi không có hình ảnh
                _context.Add(product);
                await _context.SaveChangesAsync();

                // Xử lý các tệp hình ảnh
                if (product.ImageFile != null && product.ImageFile.Count > 0)
                {
                    foreach (var file in product.ImageFile)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        var path = Path.Combine(_env.WebRootPath, "images", uniqueFileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        // Lưu mỗi đường dẫn hình ảnh duy nhất vào cơ sở dữ liệu
                        var productImage = new ProductImage
                        {
                            ImagePath = uniqueFileName,
                            ProductId = product.Id
                        };
                        _context.ProductImages.Add(productImage);
                    }
                    await _context.SaveChangesAsync();
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


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,Description,Price,image,Address,Quantity,CategoryId,ColorId,ConditionId,ProvenienceId,WarrantyId")] Product product)
        //{
        //    if (ModelState.IsValid)
        //    {

        //        // Lấy thông tin người đăng nhập
        //        var userId = _userManager.GetUserId(User);
        //        // Gán UserId cho sản phẩm
        //        product.UserId = userId;
        //        _context.Add(product);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
        //    ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", product.ColorId);
        //    ViewData["ConditionId"] = new SelectList(_context.Conditions, "Id", "ConditionName", product.ConditionId);
        //    ViewData["ProvenienceId"] = new SelectList(_context.Proveniences, "Id", "ProvenienceName", product.ProvenienceId);
        //    ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "WarrantyName", product.WarrantyId);
        //    return View(product);
        //}

        //--------------------------------------------------------------------------------------------------
        // --------------------------------------------- Sửa tin ---------------------------------------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(p => p.Images).FirstOrDefaultAsync(p => p.Id == id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Price,ImageFiles,Address,Quantity,CategoryId,ColorId,ConditionId,ProvenienceId,WarrantyId")] Product product, List<IFormFile> newImages, List<int> imagesToDelete)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Lấy thông tin người đăng nhập
                    var userId = _userManager.GetUserId(User);
                    // Gán UserId cho sản phẩm
                    product.UserId = userId;

                    if (imagesToDelete != null)
                    {
                        var imagesToRemove = _context.ProductImages
                                                    .Where(pi => imagesToDelete.Contains(pi.Id) && pi.ProductId == id)
                                                    .ToList();

                        foreach (var image in imagesToRemove)
                        {
                            // Xóa hình ảnh từ thư mục
                            var imagePath = Path.Combine(_env.WebRootPath, "images", image.ImagePath);
                            if (System.IO.File.Exists(imagePath))
                            {
                                System.IO.File.Delete(imagePath);
                            }

                            // Xóa thông tin hình ảnh khỏi cơ sở dữ liệu
                            _context.ProductImages.Remove(image);
                        }
                    }

                    if (newImages != null && newImages.Count > 0)
                    {
                        foreach (var file in newImages)
                        {
                            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            var path = Path.Combine(_env.WebRootPath, "images", uniqueFileName);

                            using (var stream = new FileStream(path, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            var productImage = new ProductImage
                            {
                                ImagePath = uniqueFileName,
                                ProductId = product.Id
                            };
                            _context.ProductImages.Add(productImage);
                        }
                    }


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
                return RedirectToAction(nameof(UserPosts));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", product.CategoryId);
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "ColorName", product.ColorId);
            ViewData["ConditionId"] = new SelectList(_context.Conditions, "Id", "ConditionName", product.ConditionId);
            ViewData["ProvenienceId"] = new SelectList(_context.Proveniences, "Id", "ProvenienceName", product.ProvenienceId);
            ViewData["WarrantyId"] = new SelectList(_context.Warranties, "Id", "WarrantyName", product.WarrantyId);
            return View(product);
        }

        //--------------------------------------------------------------------------------------------------
        // --------------------------------------------- Xóa tin ---------------------------------------------------

        [HttpPost]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        //--------------------------------------------------------------------------------------------------
        // --------------------------------------------- Chi tiết tin ---------------------------------------------------

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
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            // Lấy thông tin về người dùng từ UserManager
            var user = await _userManager.FindByIdAsync(product.UserId);

            // Đặt tên người dùng vào model
            product.UserName = user.UserName;
            return View(product);
        }
        //--------------------------------------------------------------------------------------------------


        // GET: Products
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Products.Include(p => p.Category).Include(p => p.Color).Include(p => p.Condition).Include(p => p.Provenience).Include(p => p.Warranty);
            return View(await applicationDbContext.ToListAsync());
        }

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
