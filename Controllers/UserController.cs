using System.IO.Pipes;
using GooBitAPI.Models;
using GooBitAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GooBitAPI.Controllers;


public class UserController : Controller
{
    private readonly UserService _userService;
    private readonly ParticipantService _participantService;
    private readonly EventService _eventService;

    public UserController(UserService userService, ParticipantService participantService, EventService eventService)
    {
        _userService = userService;
        _participantService = participantService;
        _eventService = eventService;
    }
        

    // [For Test] Find user controller 
    // [HttpGet]
    public async Task<ActionResult<User>> Get()
    {
        var user = await _userService.GetAsync();
        return View(user);
    }
        
    // [For Test] Find user by ID controller 
    // [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> GetById(string id)
    {
        var user = await _userService.GetById(id);

        if (user is null)
        {
            return NotFound();
        }
        return CreatedAtAction(nameof(Get),user);
    }

    // Get user profile
    public async Task<IActionResult> Profile()
    {
        var id = HttpContext.Session.GetString("userID");
        if (id == null)
        {
            return RedirectToAction("Login","User");
        }
        UserNoPassword userData = await _userService.userProfile(id);
        List<Participant> participants = await _participantService.GetByUserId(id);
        List<ShortEventDisplay> joinedEvent = new List<ShortEventDisplay>();
        foreach (Participant participant in participants)
        {
            var _event = await _eventService.GetById(participant.event_id);
            if (_event != null && _event.user_id != null)
            {
                var host_user = await _userService.GetById(_event.user_id);
                if (host_user != null && host_user.profile_img != null)
                {
                    ShortEventDisplay sEvent = _eventService.MakeSEvent(_event,host_user.firstname,host_user.lastname, host_user.profile_img);
                    joinedEvent.Add(sEvent);
                }
            }
        }
        if (userData.profile_img != null)
        {
            List<ShortEventDisplay> ownedEvent = await _eventService.GetByCreateUser(id, userData.firstname, userData.lastname, userData.profile_img);
            UserProfile userProfile = new UserProfile{
                email = userData.email,
                firstname = userData.firstname,
                lastname = userData.lastname,
                description = userData.description,
                profile_img = userData.profile_img,
                owned_event = ownedEvent,
                joined_event = joinedEvent
            };
            return Ok(userProfile);
        }
        else
        {
            Console.WriteLine("user profile image is null");
            return BadRequest();
        }
    }

    public IActionResult Register()
    {
        return View();
    }

    // Register user
    [HttpPost]
    public async Task<IActionResult> Register([FromForm]User newUser)
    {
        if (ModelState.IsValid)
        {
            if (newUser.profile_img == null)
            {
                newUser.profile_img = "default_img.png";
            }
            await _userService.CreateAsync(newUser);
            return RedirectToAction("Login","User");
        }
        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    // Login user
    [HttpPost]
    public async Task<IActionResult> LogIn([FromForm]Login login)
    {
        string? id = await _userService.Login(login);
        if (id == null)
        {
            return View();
        }
        HttpContext.Session.SetString("userID",id);
        return RedirectToAction("Index","Home");
    }

    // Logout user
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return Ok("Logout success");
    }

    public IActionResult Edit()
    {
        return View();
    }

    // Edit user
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody]UpdateUser updatedUser, IFormFile proImage)
    {
        var id = HttpContext.Session.GetString("userID");
        if (id == null)
        {
            return RedirectToAction("Login","User");
        }
        if (updatedUser.password != updatedUser.confirm_password){
            ModelState.AddModelError("PasswordValidate","Unmatch password.");
            return BadRequest();
        }
        if (proImage == null)
        {
            ModelState.AddModelError("ImageValidate","Please select an image file to upload.");
            return BadRequest();
        }

        var folderName = Path.Combine("wwwroot","profileImage");
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(),folderName);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        Guid newuuid = Guid.NewGuid();
        string newfilename = newuuid.ToString();
        string ext = System.IO.Path.GetExtension(proImage.FileName);
        newfilename = newuuid.ToString() + ext;
        updatedUser.profile_img = newfilename;
        string fileSavePath = Path.Combine(uploadsFolder, newfilename);
        using (FileStream stream = new FileStream(fileSavePath, FileMode.Create))
        {
                await proImage.CopyToAsync(stream);
        }

        var updateUser = await _userService.UpdateAsync(id,updatedUser);
        return CreatedAtAction(nameof(Get),updateUser);
    }

    // [For Test][Not use] Delete user 
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetById(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userService.RemoveAsync(id);

        return NoContent();
    }
}