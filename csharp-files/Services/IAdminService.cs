using YameApi.Models;

namespace YameApi.Services
{
    public interface IAdminService
    {
        Task<bool> IsUserAdminAsync(int userId);
        Task<DashboardStats> GetDashboardStatsAsync();
        
        // Product Management
        Task<bool> CreateProductAsync(ProductCreateRequest request);
        Task<bool> UpdateProductAsync(int productId, ProductUpdateRequest request);
        Task<bool> DeleteProductAsync(int productId);
        
        // Order Management
        Task<PaginatedList<Order>> GetAllOrdersAsync(int page, int pageSize, string? status);
        
        // User Management
        Task<PaginatedList<UserAccount>> GetAllUsersAsync(int page, int pageSize);
        Task<bool> ToggleUserRoleAsync(int userId, string role);
        
        // Category Management
        Task<List<Category>> GetCategoriesAsync();
        Task<bool> CreateCategoryAsync(string name, string slug, string? description);
        Task<bool> DeleteCategoryAsync(int categoryId);
    }
}
