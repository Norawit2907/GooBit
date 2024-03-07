using GooBitAPI.Models;
using GooBitAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
namespace GooBitAPI.Controllers;

public class EventController : Controller
{
    private readonly UserService _userService;
    private readonly ParticipantService _participantService;
    private readonly EventService _eventService;
    public EventController(EventService eventService, ParticipantService participantService, UserService userService)
    {
        _eventService = eventService;
        _participantService = participantService;
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
        if (user_id == null)
        {
            return RedirectToAction("Login","User");
        }
        var user = await _userService.GetById(user_id);
        {
            if (user == null)
            {
                return RedirectToAction("Login","User");
            }
        }
        ViewBag.UserName = $"{user.firstname} {user.lastname}";
        ViewBag.ProfileImg = $"{user.profile_img}";
        return View();
    }

    // Event/Create ---- Post method
    [HttpPost, ActionName("Create")]
    public async Task<IActionResult> ConfirmedCreate(Event newEvent, List<IFormFile> images)
    {
        string? user_id = HttpContext.Session.GetString("userID");
        if (user_id == null)
        {
            return RedirectToAction("Login","User");
        }
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

        await _eventService.CreateAsync(newEvent);
        return View("Create");
    }

    public async Task<IActionResult> EditEvent(string id)
    {
        Event? _event = await _eventService.GetById(id);
        if (_event == null)
        {
            return BadRequest();
        }
        List<Participant> participants = await _participantService.GetByEvent(id);
        List<UserStatus> allUser = [];
        int submited_user = 0;
        foreach (Participant participant in participants)
        {
            if (participant.status == "submited")
            {
                submited_user++;
            }
            User? user = await _userService.GetById(participant.user_id);
            if (user != null)
            {
                UserStatus u = new UserStatus{
                    Id = user.Id,
                    firstname = user.firstname,
                    lastname = user.lastname,
                    status = participant.status
                };
                allUser.Add(u);
            }
        }
        EditEventDisplay editEvent = new EditEventDisplay{
            Id = _event.Id,
            title = _event.title,
            description = _event.description,
            total_member = _event.total_member,
            max_member = _event.max_member,
            end_date = _event.end_date,
            event_date = _event.event_date,
            duration = _event.duration,
            googlemap_location = _event.googlemap_location,
            event_img = _event.event_img,
            category = _event.category,
            status = _event.status,
            latitude = _event.latitude,
            longitude = _event.longitude,
            available_user = _event.max_member - submited_user,
            participants = allUser
        };
        return Ok(editEvent);
    }

    public IActionResult Edit()
    {
        return View();
    }

    // [HttpPost]
    // public async Task<IActionResult> Edit()
    // {
        
    // } 

    public async Task<IActionResult> JoinedEvent(string id)
    {
        string? user_id = HttpContext.Session.GetString("userID");
        if (user_id == null)
        {
            return RedirectToAction("Login","User");
        }
        Event? _event = await _eventService.GetById(id);
        if (_event == null)
        {
            return BadRequest();
        }
        _event.total_member ++;
        await _eventService.UpdateAsync(id,_event);
        Participant participant = new Participant{
            event_id = id,
            user_id = user_id,
            status = "pending"
        };
        await _participantService.CreateAsync(participant);
        return Ok();

    }
}