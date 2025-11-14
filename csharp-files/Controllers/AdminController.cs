using Microsoft.AspNetCore.Mvc;
using YameApi.Services;
using YameApi.Models;

namespace YameApi.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IAdminService _adminService;

        public AdminController(
            IAccountService accountService,
            IProductService productService,
            IOrderService orderService,
            IAdminService adminService)
        {
            _accountService = accountService;
            _productService = productService;
            _orderService = orderService;
            _adminService = adminService;
        }

        // Check if user is admin
        private async Task<bool> IsAdminAsync()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null) return false;
            
            return await _adminService.IsUserAdminAsync(userId.Value);
        }

        // Dashboard Home
        public async Task<IActionResult> Index()
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var stats = await _adminService.GetDashboardStatsAsync();
            return View(stats);
        }

        // Product Management
        public async Task<IActionResult> Products(int page = 1)
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var query = new ProductQuery { Page = page, PageSize = 20 };
            var products = await _productService.GetProductsAsync(query);
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var categories = await _adminService.GetCategoriesAsync();
            ViewBag.Categories = categories;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductCreateRequest request)
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var success = await _adminService.CreateProductAsync(request);
            if (success)
            {
                TempData["Success"] = "Sản phẩm đã được tạo thành công";
                return RedirectToAction("Products");
            }

            TempData["Error"] = "Không thể tạo sản phẩm";
            return RedirectToAction("CreateProduct");
        }

        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var categories = await _adminService.GetCategoriesAsync();
            ViewBag.Categories = categories;
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(int id, ProductUpdateRequest request)
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var success = await _adminService.UpdateProductAsync(id, request);
            if (success)
            {
                TempData["Success"] = "Sản phẩm đã được cập nhật thành công";
                return RedirectToAction("Products");
            }

            TempData["Error"] = "Không thể cập nhật sản phẩm";
            return RedirectToAction("EditProduct", new { id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (!await IsAdminAsync())
                return Json(new { success = false, message = "Unauthorized" });

            var success = await _adminService.DeleteProductAsync(id);
            return Json(new { success, message = success ? "Xóa thành công" : "Xóa thất bại" });
        }

        // Order Management
        public async Task<IActionResult> Orders(int page = 1, string? status = null)
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var orders = await _adminService.GetAllOrdersAsync(page, 20, status);
            ViewBag.CurrentStatus = status;
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> OrderDetail(int id)
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null)
                return NotFound();

            return View(order);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, string status)
        {
            if (!await IsAdminAsync())
                return Json(new { success = false, message = "Unauthorized" });

            var success = await _orderService.UpdateOrderStatusAsync(orderId, status);
            return Json(new { success, message = success ? "Cập nhật thành công" : "Cập nhật thất bại" });
        }

        // User Management
        public async Task<IActionResult> Users(int page = 1)
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var users = await _adminService.GetAllUsersAsync(page, 20);
            return View(users);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleUserRole(int userId, string role)
        {
            if (!await IsAdminAsync())
                return Json(new { success = false, message = "Unauthorized" });

            var success = await _adminService.ToggleUserRoleAsync(userId, role);
            return Json(new { success, message = success ? "Cập nhật thành công" : "Cập nhật thất bại" });
        }

        // Category Management
        public async Task<IActionResult> Categories()
        {
            if (!await IsAdminAsync())
                return RedirectToAction("Login", "Account");

            var categories = await _adminService.GetCategoriesAsync();
            return View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(string name, string slug, string? description)
        {
            if (!await IsAdminAsync())
                return Json(new { success = false, message = "Unauthorized" });

            var success = await _adminService.CreateCategoryAsync(name, slug, description);
            return Json(new { success, message = success ? "Tạo thành công" : "Tạo thất bại" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (!await IsAdminAsync())
                return Json(new { success = false, message = "Unauthorized" });

            var success = await _adminService.DeleteCategoryAsync(id);
            return Json(new { success, message = success ? "Xóa thành công" : "Xóa thất bại" });
        }
    }
}
