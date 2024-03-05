using Microsoft.AspNetCore.Mvc;

namespace GooBitAPI.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Register()
        {
            return View();
        }
    }
}
