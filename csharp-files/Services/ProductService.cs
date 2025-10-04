using YameApi.Models;
using YameApi.Models.DTOs;

namespace YameApi.Services
{
    public class ProductService : IProductService
    {
        // Mock data - replace with actual database calls
        private readonly List<Product> _mockProducts = new()
        {
            new Product
            {
                Id = 1,
                Title = "Áo Thun Thermo Mesh",
                Slug = "ao-thun-thermo-mesh",
                ShortDescription = "Áo thun cao cấp với công nghệ Thermo Mesh thoáng mát",
                Description = "Áo thun Thermo Mesh được thiết kế đặc biệt với chất liệu cao cấp, mang lại cảm giác thoáng mát suốt cả ngày.",
                Price = 196200,
                SalePrice = 196200,
                Images = new List<string> { "/images/product-1.jpg" },
                Variants = new List<ProductVariant>
                {
                    new() { VariantId = 1, Sku = "YM-AT-001-S", Size = "S", Color = "Vàng", Stock = 10, Price = 196200 },
                    new() { VariantId = 2, Sku = "YM-AT-001-M", Size = "M", Color = "Vàng", Stock = 15, Price = 196200 },
                    new() { VariantId = 3, Sku = "YM-AT-001-L", Size = "L", Color = "Vàng", Stock = 8, Price = 196200 },
                    new() { VariantId = 4, Sku = "YM-AT-001-XL", Size = "XL", Color = "Vàng", Stock = 5, Price = 196200 }
                },
                Availability = true,
                Tags = new List<string> { "casual", "thermo-mesh", "nam" },
                PrimaryCategory = "Áo",
                Specs = new ProductSpecs { Material = "Thermo Mesh", MadeIn = "Việt Nam" },
                Reviews = new ProductReviews { Average = 4.6, Count = 120 }
            },
            new Product
            {
                Id = 2,
                Title = "Áo Hoodie Premium",
                Slug = "ao-hoodie-premium",
                ShortDescription = "Áo hoodie thời trang, chất liệu cotton cao cấp",
                Description = "Áo hoodie được thiết kế hiện đại với chất liệu cotton cao cấp, giữ ấm tốt và thoải mái.",
                Price = 450000,
                SalePrice = 380000,
                Images = new List<string> { "/images/product-2.jpg" },
                Variants = new List<ProductVariant>
                {
                    new() { VariantId = 5, Sku = "YM-HD-002-M", Size = "M", Color = "Đen", Stock = 12, Price = 380000 },
                    new() { VariantId = 6, Sku = "YM-HD-002-L", Size = "L", Color = "Đen", Stock = 20, Price = 380000 },
                    new() { VariantId = 7, Sku = "YM-HD-002-XL", Size = "XL", Color = "Đen", Stock = 7, Price = 380000 }
                },
                Availability = true,
                Tags = new List<string> { "hoodie", "premium", "streetwear" },
                PrimaryCategory = "Áo",
                Specs = new ProductSpecs { Material = "Cotton Premium", MadeIn = "Việt Nam" },
                Reviews = new ProductReviews { Average = 4.8, Count = 85 }
            }
        };

        public async Task<ProductListResponse> GetProductsAsync(ProductQuery query)
        {
            // Simulate async operation
            await Task.Delay(1);

            var filtered = _mockProducts.AsEnumerable();

            // Apply filters
            if (!string.IsNullOrEmpty(query.Q))
                filtered = filtered.Where(p => p.Title.Contains(query.Q, StringComparison.OrdinalIgnoreCase));

            if (query.PriceMin.HasValue)
                filtered = filtered.Where(p => (p.SalePrice ?? p.Price) >= query.PriceMin.Value);

            if (query.PriceMax.HasValue)
                filtered = filtered.Where(p => (p.SalePrice ?? p.Price) <= query.PriceMax.Value);

            if (query.AvailableOnly == true)
                filtered = filtered.Where(p => p.Availability);

            if (query.OnSale == true)
                filtered = filtered.Where(p => p.SalePrice.HasValue && p.SalePrice < p.Price);

            var totalCount = filtered.Count();
            var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

            var items = filtered
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            return new ProductListResponse
            {
                Items = items,
                Meta = new ResponseMeta
                {
                    TotalCount = totalCount,
                    Page = query.Page,
                    PageSize = query.PageSize,
                    TotalPages = totalPages
                },
                Facets = new ProductFacets
                {
                    Sizes = _mockProducts.SelectMany(p => p.Variants).Select(v => v.Size).Distinct().ToList(),
                    Colors = _mockProducts.SelectMany(p => p.Variants).Select(v => v.Color).Distinct().ToList(),
                    Categories = _mockProducts.Select(p => p.PrimaryCategory).Distinct().ToList()
                }
            };
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            await Task.Delay(1);
            return _mockProducts.FirstOrDefault(p => p.Id == id);
        }

        public async Task<Product?> GetBySlugAsync(string slug)
        {
            await Task.Delay(1);
            return _mockProducts.FirstOrDefault(p => p.Slug == slug);
        }
    }
}
