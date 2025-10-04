using Microsoft.AspNetCore.Mvc;
using YameApi.Models;
using YameApi.Models.DTOs;
using YameApi.Services;

namespace YameApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Get paginated list of products with filters
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ProductListResponse>> GetProducts([FromQuery] ProductQuery query)
        {
            var result = await _productService.GetProductsAsync(query);
            return Ok(result);
        }

        /// <summary>
        /// Get product by ID
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }

        /// <summary>
        /// Get product by slug
        /// </summary>
        [HttpGet("slug/{slug}")]
        public async Task<ActionResult<Product>> GetProductBySlug(string slug)
        {
            var product = await _productService.GetBySlugAsync(slug);
            if (product == null)
                return NotFound(new { message = "Product not found" });

            return Ok(product);
        }
    }
}
