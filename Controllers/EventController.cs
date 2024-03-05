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
            var user = await _userService.GetById(user_id);
            {
                if (user == null)
                {
                    return BadRequest();
                }
                else
                {
                    ViewBag.UserName = $"{user.firstname} {user.lastname}";
                    ViewBag.ProfileImg = $"{user.profile_img}";
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
    [HttpPost, ActionName("Create")]
    public async Task<IActionResult> ConfirmedCreate(Event newEvent, List<IFormFile> images)
    {
        string? user_id = HttpContext.Session.GetString("userID");
        newEvent.user_id = user_id;
        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(newEvent))
        {
            string name = descriptor.Name;
            object? value = descriptor.GetValue(newEvent);
            Console.WriteLine("{0}={1}", name, value);
        }

        if (images == null || images.Count == 0)
        {
            ModelState.AddModelError("imageFile", "Please select an image file to upload.");
            Console.WriteLine("-----no images-----");
            return View();
        }

        // store image to /wwwroot/uploadFiles
        var folderName = Path.Combine("wwwroot", "uploadFiles");
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }
        // rename file to id
        foreach (var file in images)
        {
            Guid newuuid = Guid.NewGuid();
            string newfilename = newuuid.ToString();
            string ext = System.IO.Path.GetExtension(file.FileName);
            newfilename = newuuid.ToString() + ext;
            newEvent.event_img.Add(newfilename);
            string fileSavePath = Path.Combine(uploadsFolder, newfilename);

            using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        //Console.WriteLine("yes");
        // foreach(var i in newEvent.event_img)
        // {
        //     Console.WriteLine(i);
        // }
        //Console.WriteLine("wsws");
        await _eventService.CreateAsync(newEvent);
        return View("Create");
    }
    public IActionResult Edit()
    {
        return View();
    }

}