using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GooBit.Models;
using GooBitAPI.Services;
using GooBitAPI.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace GooBitAPI.Controllers;

public class HomeController : Controller
{
    private readonly UserService _userService;
    private readonly EventService _eventService;
    
    public HomeController(EventService eventService, UserService userService)
    {
        _eventService = eventService;
        _userService = userService;
    }

    public async Task<IActionResult> Index(string category="all")
    {
        var allEvent = new List<ShortEventDisplay>{};

        // get category from service
        List<Event> _events;
        if (category == "all")
        {
            Console.WriteLine("all");
            _events = await _eventService.GetAsync();
        }
        else
        {
            Console.WriteLine(category);
            _events = await _eventService.GetByCategory(category);
        }

        if(_events == null)
        {
            return NotFound();
        }

        foreach(Event _event in _events)
        {
            Console.WriteLine(_event.title);
            var user_id = _event.user_id;
            if(user_id != null)
            {
                var user = await _userService.GetById(user_id);
                if(user != null)
                {
                    var firstname = user.firstname;
                    var lastname = user.lastname;
                    var sEvent = _eventService.MakeSEvent(_event, firstname, lastname);
                    allEvent.Add(sEvent);
                }
            }
        }
        ViewBag.ShortEventDisplay = allEvent;
        return View();
    }

    public IActionResult Privacy()
    {
        return RedirectToAction("Index","Login");
    }

    public IActionResult Post()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
