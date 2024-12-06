using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Tam_222631136.Models;

namespace Tam_222631136.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NewShopContext _context;
        public HomeController(ILogger<HomeController> logger, NewShopContext db)
        {
            _logger = logger;
            _context = db;
        }

        public IActionResult Index()
        {
            var topProducts = _context.Products
                              .OrderByDescending(p => p.UnitPrice)
                              .Take(3)
                              .ToList();
            return View(topProducts);
        }


        [HttpGet]
        public IActionResult CreateProduct()
        {
            ViewBag.ProviderId = new SelectList(_context.Providers.ToList(), "Id", "ProviderName");
            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "Id", "Name");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateProduct([Bind("Id,Name,UnitPrice,Image,Available,CategoryId,Description, ProviderId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.ProviderId = new SelectList(_context.Providers.ToList(), "Id", "ProviderName");
            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "Id", "Name", product.CategoryId);
            return View(product);
        }

        [HttpGet]
		public IActionResult SearchProducts(string keyword)
		{
			if (string.IsNullOrEmpty(keyword))
			{
				return Json(new { success = false, message = "Keyword is required" });
			}

			var products = _context.Products
								   .Where(p => p.Name.Contains(keyword))
								   .Select(p => new
								   {
									   p.Id,
									   p.Name,
									   p.UnitPrice,
									   p.Image
								   })
								   .ToList();

			return Json(new { success = true, data = products });
		}

        [HttpGet]
        public IActionResult CreateSampleProduct()
        {
            var provider = _context.Providers.FirstOrDefault();
            if (provider == null)
            {
                return Json(new { success = false, message = "No providers available to assign the product." });
            }
            // Tạo sản phẩm mẫu
            var sampleProduct = new Product
            {
                Id="23",
                Name = "Sample Product",
                UnitPrice = 100000, // Đơn giá
                Image = "sample-product.jpg", // Đường dẫn ảnh (nếu có)
                Available = true,
                CategoryId = _context.Categories.FirstOrDefault()?.Id ?? 0, // Lấy ID của danh mục đầu tiên
                ProviderId = provider.Id,
                Description = "This is a sample product for testing."
            };

            // Kiểm tra nếu CategoryId hợp lệ
            if (sampleProduct.CategoryId == 0)
            {
                return Json(new { success = false, message = "No categories available to assign the product." });
            }

            // Thêm sản phẩm mẫu vào database
            _context.Products.Add(sampleProduct);
            _context.SaveChanges();

            // Trả về thông báo thành công
            return Json(new { success = true, message = "Sample product created successfully!", product = sampleProduct });
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
