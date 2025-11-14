using YameApi.Models;
using YameApi.Models.DTOs;

namespace YameApi.Services
{
    public interface IProductService
    {
        Task<ProductListResponse> GetProductsAsync(ProductQuery query);
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetBySlugAsync(string slug);
        Task<bool> AddReviewAsync(ReviewRequest request);
    }
}
