using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}