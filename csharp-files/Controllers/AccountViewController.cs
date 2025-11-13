using Microsoft.AspNetCore.Mvc;
using YameApi.Services;

namespace YameApi.Controllers;

public class AccountViewController : Controller
{
    private readonly IAccountService _accountService;
    private readonly IOrderService _orderService;

    public AccountViewController(IAccountService accountService, IOrderService orderService)
    {
        _accountService = accountService;
        _orderService = orderService;
    }

    // My Account Page
    public async Task<IActionResult> Profile()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var user = await _accountService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        return View(user);
    }

    // Update Profile
    [HttpPost]
    public async Task<IActionResult> UpdateProfile(string fullName, string phone, string address)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var success = await _accountService.UpdateProfileAsync(userId, fullName, phone, address);
        
        if (success)
        {
            // Update session name
            if (!string.IsNullOrWhiteSpace(fullName))
                HttpContext.Session.SetString("UserName", fullName);
                
            ViewBag.SuccessMessage = "Cập nhật thông tin thành công!";
        }
        else
        {
            ViewBag.ErrorMessage = "Không thể cập nhật thông tin";
        }

        var user = await _accountService.GetUserByIdAsync(userId);
        return View("Profile", user);
    }

    // Change Password Page
    public IActionResult ChangePassword()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr))
        {
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    // Change Password Action
    [HttpPost]
    public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        if (newPassword != confirmPassword)
        {
            ViewBag.Error = "Mật khẩu mới và xác nhận không khớp";
            return View();
        }

        var success = await _accountService.ChangePasswordAsync(userId, currentPassword, newPassword);
        
        if (success)
        {
            ViewBag.Success = "Đổi mật khẩu thành công!";
        }
        else
        {
            ViewBag.Error = "Mật khẩu hiện tại không đúng";
        }

        return View();
    }

    // Order History
    public async Task<IActionResult> Orders()
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var orders = await _orderService.GetOrdersByUserIdAsync(userId);
        return View(orders);
    }

    // Order Detail
    public async Task<IActionResult> OrderDetail(int id)
    {
        var userIdStr = HttpContext.Session.GetString("UserId");
        if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
        {
            return RedirectToAction("Login", "Account");
        }

        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null || order.CustomerId != userId)
        {
            return NotFound();
        }

        return View(order);
    }

    // Forgot Password
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // Request Password Reset
    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            ViewBag.Error = "Vui lòng nhập email";
            return View();
        }

        var token = await _accountService.GeneratePasswordResetTokenAsync(email);
        
        if (token != null)
        {
            // In production, send email with reset link
            // For now, we'll just display the token
            ViewBag.Success = $"Mã đặt lại mật khẩu đã được tạo. Vui lòng kiểm tra email của bạn.";
            ViewBag.ResetToken = token; // Remove this in production
            ViewBag.Email = email;
        }
        else
        {
            ViewBag.Error = "Email không tồn tại trong hệ thống";
        }

        return View();
    }

    // Reset Password Page
    public IActionResult ResetPassword(string email, string token)
    {
        ViewBag.Email = email;
        ViewBag.Token = token;
        return View();
    }

    // Reset Password Action
    [HttpPost]
    public async Task<IActionResult> ResetPassword(string email, string token, string newPassword, string confirmPassword)
    {
        if (newPassword != confirmPassword)
        {
            ViewBag.Error = "Mật khẩu mới và xác nhận không khớp";
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }

        var success = await _accountService.ResetPasswordAsync(email, token, newPassword);
        
        if (success)
        {
            ViewBag.Success = "Đặt lại mật khẩu thành công! Bạn có thể đăng nhập ngay bây giờ.";
            return View("ResetPasswordSuccess");
        }
        else
        {
            ViewBag.Error = "Mã đặt lại mật khẩu không hợp lệ hoặc đã hết hạn";
            ViewBag.Email = email;
            ViewBag.Token = token;
            return View();
        }
    }
}
