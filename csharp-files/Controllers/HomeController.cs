using Microsoft.AspNetCore.Mvc;
using YameApi.Services;

namespace YameApi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;

        public HomeController(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            // Get featured products for homepage
            var query = new Models.DTOs.ProductQuery { PageSize = 8 };
            var result = await _productService.GetProductsAsync(query);
            return View(result.Items);
        }
    }
}
