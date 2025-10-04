using YameApi.Models;

namespace YameApi.Services
{
    public class CartService : ICartService
    {
        // In-memory cart storage - replace with database/Redis in production
        private static readonly Dictionary<string, Cart> _carts = new();

        public async Task<Cart> CreateCartAsync()
        {
            await Task.Delay(1);
            var cart = new Cart
            {
                CartId = Guid.NewGuid().ToString(),
                Items = new List<CartItem>()
            };
            _carts[cart.CartId] = cart;
            return cart;
        }

        public async Task<Cart?> GetCartAsync(string cartId)
        {
            await Task.Delay(1);
            return _carts.TryGetValue(cartId, out var cart) ? cart : null;
        }

        public async Task<Cart?> AddItemAsync(string cartId, int variantId, int quantity)
        {
            await Task.Delay(1);
            if (!_carts.TryGetValue(cartId, out var cart))
                return null;

            // Mock: Find variant and product (replace with actual DB lookup)
            var existingItem = cart.Items.FirstOrDefault(i => i.VariantId == variantId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                existingItem.LineTotal = existingItem.UnitPrice * existingItem.Quantity;
            }
            else
            {
                // Add new item (simplified - need actual product lookup)
                var newItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    VariantId = variantId,
                    Quantity = quantity,
                    UnitPrice = 196200, // Mock price
                    LineTotal = 196200 * quantity
                };
                cart.Items.Add(newItem);
            }

            RecalculateCart(cart);
            return cart;
        }

        public async Task<Cart?> UpdateItemAsync(string cartId, string itemId, int quantity)
        {
            await Task.Delay(1);
            if (!_carts.TryGetValue(cartId, out var cart))
                return null;

            var item = cart.Items.FirstOrDefault(i => i.ItemId == itemId);
            if (item == null)
                return null;

            item.Quantity = quantity;
            item.LineTotal = item.UnitPrice * quantity;
            RecalculateCart(cart);
            return cart;
        }

        public async Task<Cart?> RemoveItemAsync(string cartId, string itemId)
        {
            await Task.Delay(1);
            if (!_carts.TryGetValue(cartId, out var cart))
                return null;

            cart.Items.RemoveAll(i => i.ItemId == itemId);
            RecalculateCart(cart);
            return cart;
        }

        private void RecalculateCart(Cart cart)
        {
            cart.SubTotal = cart.Items.Sum(i => i.LineTotal);
            cart.Total = cart.SubTotal - cart.Discount + cart.Shipping + cart.Tax;
        }
    }
}
