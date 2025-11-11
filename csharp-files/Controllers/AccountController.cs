using Microsoft.AspNetCore.Mvc;
using YameApi.Services;

namespace YameApi.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ViewBag.Error = "Email và mật khẩu không được để trống";
            return View();
        }

        var user = await _accountService.LoginAsync(email, password);
        if (user == null)
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
    public async Task<IActionResult> Register(string email, string password, string confirmPassword, string fullName, string phone)
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

        var newUser = await _accountService.RegisterAsync(email, password, fullName, phone);
        
        if (newUser == null)
        {
            ViewBag.Error = "Email đã được sử dụng";
            return View();
        }

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
}
