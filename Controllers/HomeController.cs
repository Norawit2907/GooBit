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
    private readonly CommentService _commentService;
    private readonly ParticipantService _participantService;
    private readonly ReplyService _replyService;
    public HomeController(EventService eventService, UserService userService, CommentService commentService, ParticipantService participantService, ReplyService replyService)
    {
        _eventService = eventService;
        _userService = userService;
        _commentService = commentService;
        _participantService = participantService;
        _replyService = replyService;
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
                if(user != null && user.profile_img != null)
                {
                    var firstname = user.firstname;
                    var lastname = user.lastname;
                    var sEvent = _eventService.MakeSEvent(_event, firstname, lastname, user.profile_img);
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

    public async Task<IActionResult> Post(string id)
    {
        string? user_id = HttpContext.Session.GetString("userID");
        Console.WriteLine(id);
        Event? _event = await _eventService.GetById(id);
        if(_event == null || _event.Id == null)     {   return NotFound();}

        User? _user = await _userService.GetById(_event.user_id);
        if (_user == null || _user.Id == null)      {   return NotFound();}

        List<Comment>? _comments = await _commentService.GetByEventId(_event.Id);
        if (_comments == null)                      {   return NotFound();}
        List<Participant>? _participants = await _participantService.GetByEventId(_event.Id);
        if (_participants == null)                  {   return NotFound();}

        List<Reply> _replies = new List<Reply>{};
        foreach(Comment _comment in _comments)
        {
            if (_comment.Id != null)
            {
                var onecommentreply = await _replyService.GetRepliesAsyncByCommentId(_comment.Id);
                if (onecommentreply != null)
                {
                    foreach (Reply _replyincomment in onecommentreply)
                    {
                        _replies.Add(_replyincomment);
                    }
                }
            
            }
        }
        EventDisplay _eventdisplay = _eventService.MakeEventDisplay(_event, _user, _comments, _participants, _replies);
        if (_eventdisplay == null)  {   return NotFound();}
        Console.WriteLine(_eventdisplay.title);

        ViewBag.EventDisplay = _eventdisplay;
        ViewBag.user_id = user_id;
        return View();
    }

    [HttpPost]
    public async Task<object> CreateComment([FromBody]Comment newComment)
    {
        Console.WriteLine(newComment.ToString());
        await _commentService.CreateAsync(newComment);

        return ViewData["Comments"] = "Sucess";
    }

    public async Task<object> CreateReply([FromBody]Reply newReply)
    {
        Console.WriteLine(newReply.ToString());
        await _replyService.CreateAsync(newReply);
        
        return ViewData["Replies"] = "Sucess";
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
