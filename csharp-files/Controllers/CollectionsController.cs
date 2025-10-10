using Microsoft.AspNetCore.Mvc;

namespace YameApi.Controllers
{
    public class CollectionsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
