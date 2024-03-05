using GooBitAPI.Models;
using GooBitAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
namespace GooBitAPI.Controllers;

public class EventController : Controller
{
    private readonly UserService _userService;
    private readonly EventService _eventService;
    public EventController(EventService eventService, UserService userService)
    {
        _eventService = eventService;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Event/Create ---- Get method
    [HttpGet, ActionName("Create")]
    public async Task<IActionResult> Create()
    {
        string? user_id = HttpContext.Session.GetString("userID");
        if (!string.IsNullOrEmpty(user_id))
        {
            var user = await _userService.GetAsync(user_id);
            {
                if (user == null)
                {
                    return BadRequest();
                }
            }   
        }
        // when current user is not found
        // else{
        //     return BadRequest();
        // }
        return View();
    }

    // Event/Create ---- Post method
    [HttpPost]
    public async Task<IActionResult> ConfirmedCreate(Event newEvent)
    {
        foreach(PropertyDescriptor descriptor in TypeDescriptor.GetProperties(newEvent))
        {
            string name = descriptor.Name;
            object? value = descriptor.GetValue(newEvent);
            Console.WriteLine("{0}={1}", name, value);
        }
        
        Console.WriteLine("yes");
        foreach(var i in newEvent.event_img)
        {
            Console.WriteLine(i);
        }
        Console.WriteLine("wsws");
        await _eventService.CreateAsync(newEvent);
        return View("create");
    }
}