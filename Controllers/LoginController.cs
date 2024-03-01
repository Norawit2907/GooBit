using Microsoft.AspNetCore.Mvc;

namespace BasicASP.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
