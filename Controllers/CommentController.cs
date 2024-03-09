using GooBitAPI.Models;
using GooBitAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GooBitAPI.Controllers;


public class CommentController : Controller
{
    private readonly CommentService _commentService;

    public CommentController(CommentService commentService) =>
        _commentService = commentService;



    [HttpGet]
    public async Task<ActionResult<Comment>> Get()
    {
        var comment = await _commentService.GetAsync();
        return View(comment);
    }
        

    // [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Comment>> GetById(string id)
    {
        var comment = await _commentService.GetAsync(id);

        if (comment is null)
        {
            return NotFound();
        }
        return comment;
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]Comment newComment)
    {
        Console.WriteLine(newComment.ToString());
        await _commentService.CreateAsync(newComment);

        return RedirectToAction("Home", "Post", new { Id = newComment.event_id});
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, [FromBody]Comment updatedComment)
    {
        var comment = await _commentService.GetAsync(id);

        if (comment is null)
        {
            return NotFound();
        }

        updatedComment.Id = comment.Id;

        await _commentService.UpdateAsync(id, updatedComment);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var comment = await _commentService.GetAsync(id);

        if (comment is null)
        {
            return NotFound();
        }

        await _commentService.RemoveAsync(id);

        return NoContent();
    }
}