using Microsoft.AspNetCore.Mvc;
using YameApi.Models.DTOs;
using YameApi.Services;

namespace YameApi.Controllers
{
    [Route("Products")]
    public class ProductsViewController : Controller
    {
        private readonly IProductService _productService;

        public ProductsViewController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("")]
        public async Task<IActionResult> Index([FromQuery] ProductQuery query)
        {
            var result = await _productService.GetProductsAsync(query);
            return View(result);
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> Detail(string slug)
        {
            var product = await _productService.GetBySlugAsync(slug);
            if (product == null)
                return NotFound();

            return View(product);
        }
    }
}
