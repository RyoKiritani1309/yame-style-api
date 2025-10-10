using Microsoft.AspNetCore.Mvc;

namespace YameApi.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
