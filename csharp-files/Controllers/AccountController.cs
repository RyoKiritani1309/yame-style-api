using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace YameApi.Controllers;

public class AccountController : Controller
{
    // In-memory user storage for demo (replace with database in production)
    private static List<UserAccount> _users = new List<UserAccount>();

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Email và mật khẩu không được để trống";
            return View();
        }

        var user = _users.FirstOrDefault(u => u.Email == email);
        if (user == null || !VerifyPassword(password, user.PasswordHash))
        {
            ViewBag.Error = "Email hoặc mật khẩu không đúng";
            return View();
        }

        // Set session
        HttpContext.Session.SetString("UserId", user.UserId.ToString());
        HttpContext.Session.SetString("UserEmail", user.Email);
        HttpContext.Session.SetString("UserName", user.FullName ?? "User");

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(string email, string password, string confirmPassword, string fullName, string phone)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Email và mật khẩu không được để trống";
            return View();
        }

        if (password != confirmPassword)
        {
            ViewBag.Error = "Mật khẩu xác nhận không khớp";
            return View();
        }

        if (_users.Any(u => u.Email == email))
        {
            ViewBag.Error = "Email đã được sử dụng";
            return View();
        }

        var newUser = new UserAccount
        {
            UserId = _users.Count + 1,
            Email = email,
            PasswordHash = HashPassword(password),
            FullName = fullName,
            Phone = phone,
            CreatedAt = DateTime.Now
        };

        _users.Add(newUser);

        // Auto login after registration
        HttpContext.Session.SetString("UserId", newUser.UserId.ToString());
        HttpContext.Session.SetString("UserEmail", newUser.Email);
        HttpContext.Session.SetString("UserName", newUser.FullName ?? "User");

        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    // Password hashing utilities
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "YAME_SALT"));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    private bool VerifyPassword(string password, string hash)
    {
        var newHash = HashPassword(password);
        return newHash == hash;
    }
}

// User model
public class UserAccount
{
    public int UserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string? FullName { get; set; }
    public string? Phone { get; set; }
    public DateTime CreatedAt { get; set; }
}
