using YameApi.Models;

namespace YameApi.Services
{
    public interface ICartService
    {
        Task<Cart> CreateCartAsync();
        Task<Cart?> GetCartAsync(string cartId);
        Task<Cart?> AddItemAsync(string cartId, int variantId, int quantity);
        Task<Cart?> UpdateItemAsync(string cartId, string itemId, int quantity);
        Task<Cart?> RemoveItemAsync(string cartId, string itemId);
    }
}
