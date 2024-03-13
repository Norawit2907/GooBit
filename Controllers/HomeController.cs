using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GooBit.Models;
using GooBitAPI.Services;
using GooBitAPI.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Net;

namespace GooBitAPI.Controllers;

public class HomeController : Controller
{
    private readonly UserService _userService;
    private readonly EventService _eventService;
    private readonly CommentService _commentService;
    private readonly ParticipantService _participantService;
    private readonly ReplyService _replyService;
    private readonly NotificationService _notificationService;
    public HomeController(EventService eventService, UserService userService, CommentService commentService, ParticipantService participantService, ReplyService replyService, NotificationService notificationService)
    {
        _eventService = eventService;
        _userService = userService;
        _commentService = commentService;
        _participantService = participantService;
        _replyService = replyService;
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Noti()
    {
        //Update event status
        List<Event> closedEvents = await _eventService.UpdateCloseEvent();
        foreach (Event closeEvent in closedEvents)
        {
            if(closeEvent.Id != null)
            {
                List<Participant> participants = await _participantService.GetByEventId(closeEvent.Id);
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
                            if (p.Id != null){await _participantService.UpdateAsync(p.Id,p);}
                        }
                    }
                }
                await _notificationService.CreateNoti(closeEvent.user_id,closeEvent.Id,"Closed");
            }
        }

        string? user_id = HttpContext.Session.GetString("userID");
        if (user_id == null)
        {
            return RedirectToAction("Login","User");
        }
        List<Notification>? Notification = await _notificationService.GetAsyncByUserId(user_id);
        List<ShowNotification> _ShowNoti = new List<ShowNotification>{};
        if (Notification != null)
        {
            foreach(var _noti in Notification)
            {
                User? user = await _userService.GetById(_noti.user_id);
                Event? evnt = await _eventService.GetById(_noti.event_id);
                if (user != null && evnt != null && user.profile_img != null)
                {
                    ShowNotification newshownoti = _notificationService.MakeSNotification(_noti, user.firstname, user.lastname, evnt.title, user.profile_img);
                    _ShowNoti.Add(newshownoti);
                }
            }
        }
        string? userid = HttpContext.Session.GetString("userID");
        if (userid == null)
        {
            return RedirectToAction("Login","User");
        }
        var unow = await _userService.GetById(userid);
        if(unow == null){
            return RedirectToAction("Login","User");
        }
        ViewBag.first = unow.firstname;
        ViewBag.last = unow.lastname;
        ViewBag.image = unow.profile_img;
        ViewBag.allnoti = _ShowNoti ;
        return View();
    }
    
    public async Task<IActionResult> Index(string category="All")
    {
        //Update event status
        List<Event> closedEvents = await _eventService.UpdateCloseEvent();
        foreach (Event closeEvent in closedEvents)
        {
            if(closeEvent.Id != null)
            {
                List<Participant> participants = await _participantService.GetByEventId(closeEvent.Id);
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
                            if (p.Id != null){await _participantService.UpdateAsync(p.Id,p);}
                        }
                    }
                }
                await _notificationService.CreateNoti(closeEvent.user_id,closeEvent.Id,"Closed");
            }
        }

        var allEvent = new List<ShortEventDisplay>{};

        // get category from service
        List<Event> _events;
        if (category == "All")
        {
            Console.WriteLine("All");
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
        string? userid = HttpContext.Session.GetString("userID");
        if (userid == null)
        {
            return RedirectToAction("Login","User");
        }
        var unow = await _userService.GetById(userid);
        if(unow == null){
            return RedirectToAction("Login","User");
        }
        ViewBag.first = unow.firstname;
        ViewBag.last = unow.lastname;
        ViewBag.image = unow.profile_img;
        ViewBag.showcategory = category;
        ViewBag.ShortEventDisplay = allEvent;
        return View();
    }

    public IActionResult Privacy()
    {
        return RedirectToAction("Index","Login");
    }

    public async Task<IActionResult> Post(string id)
    {
        //Update event status
        List<Event> closedEvents = await _eventService.UpdateCloseEvent();
        foreach (Event closeEvent in closedEvents)
        {
            if(closeEvent.Id != null)
            {
                List<Participant> participants = await _participantService.GetByEventId(closeEvent.Id);
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
                            if (p.Id != null){await _participantService.UpdateAsync(p.Id,p);}
                        }
                    }
                }
                await _notificationService.CreateNoti(closeEvent.user_id,closeEvent.Id,"Closed");
            }
        }
        
        string? user_id = HttpContext.Session.GetString("userID");
        Event? _event = await _eventService.GetById(id);
        if(user_id == null)
        {
            return RedirectToAction("Login","User");
        }
        if(_event == null || _event.Id == null)     {   return NotFound();}

        User? _user = await _userService.GetById(_event.user_id);
        if (_user == null || _user.Id == null)      {   return NotFound();}

        List<Comment>? _comments = await _commentService.GetByEventId(_event.Id);
        if (_comments == null)                      {   return NotFound();}

        List<ShowComment> _showcomments = new List<ShowComment>{};
        foreach(Comment _comment in _comments)
        {
            if (_comment.Id != null)
            {
                List<Reply> _replies = await _replyService.GetRepliesAsyncByCommentId(_comment.Id);
                
                ShowComment SC = _commentService.MakeSComment(_comment, _comment.firstname, _comment.lastname, _replies);
                _showcomments.Add(SC);
            }
        }

        List<Participant>? _participants = await _participantService.GetByEventId(_event.Id);
        if (_participants == null)                  {   return NotFound();}
        List<ShowParticipant> _showparticipants = new List<ShowParticipant>{};
        foreach(var _part in _participants)
        {
            
            User? _PU = await _userService.GetById(_part.user_id);
            if (_PU != null && _PU.profile_img != null)
            {
                ShowParticipant SP = _participantService.MakeShowParticipant(_part, _PU.firstname, _PU.lastname, _PU.profile_img);
                _showparticipants.Add(SP);
            }
        }
        foreach(var s in _showparticipants)
        {
            Console.WriteLine(s);
        }
        EventDisplay _eventdisplay = _eventService.MakeEventDisplay(_event, _user, _showcomments, _showparticipants);
        if (_eventdisplay == null)  {   return NotFound();}
        User? cur_user = await _userService.GetById(user_id);
        if (cur_user == null)
        {
            return RedirectToAction("Login","User");
        }
        string? userid = HttpContext.Session.GetString("userID");
        if (userid == null)
        {
            return RedirectToAction("Login","User");
        }
        var unow = await _userService.GetById(userid);
        if(unow == null){
            return RedirectToAction("Login","User");
        }
        ViewBag.first = unow.firstname;
        ViewBag.last = unow.lastname;
        ViewBag.image = unow.profile_img;
        ViewBag.EventDisplay = _eventdisplay;
        ViewBag.user_id = user_id;
        ViewBag.user_firstname = cur_user.firstname;
        ViewBag.user_lastname = cur_user.lastname;
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

    [HttpPost]
    public async Task<IActionResult> CreateParticipant([FromBody]Event_Id event_id)
    {
        string id = event_id.Id;
        string? user_id = HttpContext.Session.GetString("userID");
        if (user_id == null)
        {
            return RedirectToAction("Login","User");
        }
        Event? _event = await _eventService.GetById(id);
        if (_event == null)
        {
            return BadRequest("What do you looking for");
        }
        if (user_id == _event.user_id)
        {
            return BadRequest("What do you looking for");
        }
        Participant? check_p = await _participantService.GetByEU(user_id,id);
        if (check_p != null || _event.status == false)
        {
            return BadRequest("Can not do it");
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

    public async Task<IActionResult> DeleteComment(string id)
    {   
        Comment? _comment = await _commentService.GetAsync(id);
        if (_comment == null)
        {
            return BadRequest();
        }
        List<Reply>? _replies = await _replyService.GetRepliesAsyncByCommentId(id);
        foreach(var r in _replies)
        {
            if (r != null && r.Id != null)
            {
            await _replyService.RemoveAsync(r.Id);
            }
        }
        await _commentService.RemoveAsync(id);
        return Ok();
    }

    


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}