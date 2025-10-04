using Microsoft.AspNetCore.Mvc;
using YameApi.Models;
using YameApi.Services;

namespace YameApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Create a new cart
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Cart>> CreateCart()
        {
            var cart = await _cartService.CreateCartAsync();
            return Ok(cart);
        }

        /// <summary>
        /// Get cart by ID
        /// </summary>
        [HttpGet("{cartId}")]
        public async Task<ActionResult<Cart>> GetCart(string cartId)
        {
            var cart = await _cartService.GetCartAsync(cartId);
            if (cart == null)
                return NotFound(new { message = "Cart not found" });

            return Ok(cart);
        }

        /// <summary>
        /// Add item to cart
        /// </summary>
        [HttpPost("{cartId}/items")]
        public async Task<ActionResult<Cart>> AddItem(string cartId, [FromBody] AddCartItemRequest request)
        {
            var cart = await _cartService.AddItemAsync(cartId, request.VariantId, request.Quantity);
            if (cart == null)
                return NotFound(new { message = "Cart not found" });

            return Ok(cart);
        }

        /// <summary>
        /// Update cart item quantity
        /// </summary>
        [HttpPut("{cartId}/items/{itemId}")]
        public async Task<ActionResult<Cart>> UpdateItem(string cartId, string itemId, [FromBody] UpdateCartItemRequest request)
        {
            var cart = await _cartService.UpdateItemAsync(cartId, itemId, request.Quantity);
            if (cart == null)
                return NotFound(new { message = "Cart or item not found" });

            return Ok(cart);
        }

        /// <summary>
        /// Remove item from cart
        /// </summary>
        [HttpDelete("{cartId}/items/{itemId}")]
        public async Task<ActionResult<Cart>> RemoveItem(string cartId, string itemId)
        {
            var cart = await _cartService.RemoveItemAsync(cartId, itemId);
            if (cart == null)
                return NotFound(new { message = "Cart or item not found" });

            return Ok(cart);
        }
    }

    public class AddCartItemRequest
    {
        public int VariantId { get; set; }
        public int Quantity { get; set; } = 1;
    }

    public class UpdateCartItemRequest
    {
        public int Quantity { get; set; }
    }
}
