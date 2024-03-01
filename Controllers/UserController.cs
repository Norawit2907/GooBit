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
        User? user = await _userService.GetByEmail(login);
        if (user == null)
        {
            return Unauthorized("Not found user");
        }
        if (user.password != login.password || user.Id == null)
        {
            return Unauthorized("Not found user");
        }
        HttpContext.Session.SetString("userID",user.Id);
        return Ok("Login success");
    }

    // Log out
    // public async Task<IActionResult> LogOut()
    // {
    //     HttpContext.Session.Remove("userID");
    //     return Ok("Logout success");
    // }

    // [Not done]Update user
    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, [FromBody]User updatedUser)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await _userService.UpdateAsync(id, updatedUser);

        return NoContent();
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