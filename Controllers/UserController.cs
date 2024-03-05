using GooBitAPI.Models;
using GooBitAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GooBitAPI.Controllers;


public class UserController : Controller
{
    private readonly UserService _userService;

    public UserController(UserService userService) =>
        _userService = userService;

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
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }
        return user;
    }

    // Register user
    [HttpPost]
    public async Task<IActionResult> Register([FromBody]User newUser)
    {
        if (ModelState.IsValid)
        {
            await _userService.CreateAsync(newUser);
            return CreatedAtAction(nameof(Get), new { id = newUser.Id }, newUser);
        }
        return NotFound();
    }

    // Login user
    [HttpPost]
    public async Task<IActionResult> LogIn([FromBody]Login login)
    {
        string? id = await _userService.Login(login);
        if (id == null)
        {
            return Unauthorized("Not found user");
        }
        HttpContext.Session.SetString("userID",id);
        return Ok("Login success");
    }

    // Log out
    public IActionResult LogOut()
    {
        HttpContext.Session.Clear();
        return Ok("Logout success");
    }

    // [Not done]Update user
    [HttpPut]
    public async Task<IActionResult> Update([FromBody]UpdateUser updatedUser)
    {
        var id = HttpContext.Session.GetString("userID");
        Console.WriteLine(id);
        if (id == null)
        {
            return Unauthorized("Go back to login");
        }
        if (updatedUser.password != updatedUser.confirm_password){
            return BadRequest("Password not match");
        }
        var updateUser = await _userService.UpdateAsync(id,updatedUser);
        return CreatedAtAction(nameof(Get),updateUser);
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userService.RemoveAsync(id);

        return NoContent();
    }
}