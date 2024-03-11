using DnsClient.Protocol;
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
    private readonly NotificationService _notificationService;
    public EventController(EventService eventService, ParticipantService participantService, UserService userService, NotificationService notificationService)
    {
        _eventService = eventService;
        _participantService = participantService;
        _userService = userService;
        _notificationService = notificationService;
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
            return RedirectToAction("Login", "User");

        }
        var user = await _userService.GetById(user_id);
        {
            if (user == null)
            {

                return RedirectToAction("Login", "User");

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

            return RedirectToAction("Login", "User");
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
        var folderName = Path.Combine("wwwroot", "uploadFiles/EventImage");
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

    [HttpGet, ActionName("Edit")]
    public async Task<IActionResult> Edit(string id)
    {
        string? user_id = HttpContext.Session.GetString("userID");
        if (user_id == null)
        {
            return RedirectToAction("Login", "User");

        }
        User? hostuser = await _userService.GetById(user_id);
        if (hostuser == null){return RedirectToAction("Login", "User");}
        Event? _event = await _eventService.GetById(id);
        if (_event == null)
        {
            return BadRequest("What do you looking for");
        }
        if (user_id != _event.user_id)
        {
            return BadRequest("What do you looking for");
        }
        List<Participant> participants = await _participantService.GetByEventId(id);
        List<UserStatus> submittedUser = [];
        List<UserStatus> allparticipant = [];
        int submited_user = 0;
        foreach (Participant participant in participants)
        {
            if (participant.status == "submitted")
            {
                submited_user++;
                User? user = await _userService.GetById(participant.user_id);
                if (user != null)
                {
                    UserStatus u = new UserStatus
                    {
                        Id = user.Id,
                        firstname = user.firstname,
                        lastname = user.lastname,
                        status = participant.status
                    };
                    submittedUser.Add(u);
                }
            }
            if (participant.status != null)
            {
                User? user = await _userService.GetById(participant.user_id);
                if (user != null)
                {
                    UserStatus u = new UserStatus
                    {
                        Id = user.Id,
                        firstname = user.firstname,
                        lastname = user.lastname,
                        status = participant.status
                    };
                    allparticipant.Add(u);
                }
            }
        }
        EditEventDisplay editEvent = new EditEventDisplay
        {

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
            available_user = _event.max_member,
            submitted_user = submittedUser,
            participants = allparticipant
        };
        ViewBag.UserName = $"{hostuser.firstname} {hostuser.lastname}";
        ViewBag.ProfileImg = $"{hostuser.profile_img}";
        return View(editEvent);
    }

    [HttpPost, ActionName("Edit")]
    public async Task<IActionResult> Edit(string id, UpdatedEvent updatedEvent, List<IFormFile> images)
    {
        string? user_id = HttpContext.Session.GetString("userID");
        if (user_id == null)
        {
            return RedirectToAction("Login", "User");

        }
        Event? _event = await _eventService.GetById(id);
        if (_event == null)
        {
            return BadRequest("What do you looking for");
        }
        if (user_id != _event.user_id)
        {
            return BadRequest("What do you looking for");
        }
        Event newEvent = new Event
        {
            Id = id,
            title = updatedEvent.title,
            description = updatedEvent.description,
            total_member = _event.total_member,
            max_member = updatedEvent.max_member,
            end_date = updatedEvent.end_date,
            event_date = updatedEvent.event_date,
            duration = updatedEvent.duration,
            googlemap_location = updatedEvent.googlemap_location,
            event_img = [],
            category = updatedEvent.category,
            status = updatedEvent.status == "open",
            user_id = _event.user_id,
            latitude = updatedEvent.latitude,
            longitude = updatedEvent.longitude
        };

        if (images == null || images.Count == 0)
        {
            List<string> oImg = updatedEvent.previous_image.Split(",").ToList();
            newEvent.event_img = oImg;
        } else 
        {
            var folderName = Path.Combine("wwwroot", "uploadFiles");
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }
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
        }

        if (updatedEvent.submitted_user != null || updatedEvent.previous_submitted_user != null)
        {
            int rejected_user = await _participantService.statusChanger(id,updatedEvent.previous_submitted_user,updatedEvent.submitted_user);
            newEvent.total_member -= rejected_user;
        }

        await _eventService.UpdateAsync(id,newEvent);

        if (updatedEvent.status != "open")
        {
            List<Participant> participants = await _participantService.GetByEventId(id);
            foreach (Participant p in participants)
            {
                if (p.status == "submitted")
                { await _notificationService.CreateNoti(p.user_id,p.event_id,"submitted"); } 
                else if (p.status == "rejected" || p.status == "pending") 
                {
                    await _notificationService.CreateNoti(p.user_id,p.event_id,"rejected");
                    if (p.status == "pending")
                    {
                        p.status = "rejected";
                        if (p.Id != null) { await _participantService.UpdateAsync(p.Id, p); }
                    }
                }
            }
            await _notificationService.CreateNoti(user_id,id,"Closed");
        }
        return Ok();
    } 
}