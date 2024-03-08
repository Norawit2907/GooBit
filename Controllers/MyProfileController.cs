using Microsoft.AspNetCore.Mvc;

namespace BasicASP.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
    }
}
