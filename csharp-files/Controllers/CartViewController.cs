using Microsoft.AspNetCore.Mvc;
using YameApi.Services;

namespace YameApi.Controllers
{
    public class CartViewController : Controller
    {
        private readonly ICartService _cartService;

        public CartViewController(ICartService cartService)
        {
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            // Get cart ID from session or cookie
            var cartId = HttpContext.Session.GetString("CartId");
            
            if (string.IsNullOrEmpty(cartId))
            {
                // Create new cart
                var newCart = await _cartService.CreateCartAsync();
                HttpContext.Session.SetString("CartId", newCart.CartId);
                ViewBag.Cart = newCart;
            }
            else
            {
                var cart = await _cartService.GetCartAsync(cartId);
                if (cart == null)
                {
                    // Cart not found, create new one
                    var newCart = await _cartService.CreateCartAsync();
                    HttpContext.Session.SetString("CartId", newCart.CartId);
                    ViewBag.Cart = newCart;
                }
                else
                {
                    ViewBag.Cart = cart;
                }
            }

            return View();
        }

        public IActionResult Checkout()
        {
            var cartId = HttpContext.Session.GetString("CartId");
            if (string.IsNullOrEmpty(cartId))
            {
                return RedirectToAction("Index");
            }

            // Pass user info if logged in
            var userId = HttpContext.Session.GetString("UserId");
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userName = HttpContext.Session.GetString("UserName");

            ViewBag.UserId = userId;
            ViewBag.UserEmail = userEmail;
            ViewBag.UserName = userName;
            ViewBag.CartId = cartId;

            return View();
        }

        public IActionResult OrderConfirmation(int orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}
