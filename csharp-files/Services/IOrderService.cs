using YameApi.Models;

namespace YameApi.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(CheckoutRequest request, int? userId);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string status);
    }
}
