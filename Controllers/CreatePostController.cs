using GooBitAPI.Models;
using GooBitAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GooBitAPI.Controllers;

public class CreatePostController : Controller{
    public IActionResult Index()
    {
        return View();
    }

}