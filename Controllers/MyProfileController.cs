using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GooBit.Models;
using GooBitAPI.Services;
using GooBitAPI.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using System.Net;

namespace BasicASP.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserService _userService;
        private readonly EventService _eventService;
        private readonly CommentService _commentService;
        private readonly ParticipantService _participantService;
        private readonly ReplyService _replyService;
        private readonly NotificationService _notificationService;
        public ProfileController(EventService eventService, UserService userService, CommentService commentService, ParticipantService participantService, ReplyService replyService, NotificationService notificationService)
        {
            _eventService = eventService;
            _userService = userService;
            _commentService = commentService;
            _participantService = participantService;
            _replyService = replyService;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index(string id)
    {
        //Update event status
        List<Event> closedEvents = await _eventService.UpdateCloseEvent();
        foreach (Event closeEvent in closedEvents)
        {
            if(closeEvent.Id != null)
            {
                List<Participant> participants = await _participantService.GetByEventId(closeEvent.Id);
                int rUser = 0;
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
                            rUser ++;
                        }
                    }
                }
                await _notificationService.CreateNoti(closeEvent.user_id,closeEvent.Id,"Closed");
                closeEvent.total_member -= rUser;
                await _eventService.UpdateAsync(closeEvent.Id,closeEvent);
            }
        }

        var my_id = HttpContext.Session.GetString("userID");
        if (my_id == null)
        {
            return RedirectToAction("Login","User");
        }
        if (id == null)
        {
            id = my_id;
        }
        var unow = await _userService.GetById(my_id);
        if(unow == null || unow.Id == null){
            return RedirectToAction("Login","User");
        }

        var proUser = await _userService.userProfile(id);
        if(proUser == null || proUser.Id == null){
            return BadRequest("What are you doing");
        }

        var allEvent = new List<ShortEventDisplay>{};

        List<Event> _events = await _eventService.GetByUserId(proUser.Id);

        if(_events == null)
        {
            return NotFound();
        }

        foreach(Event _event in _events)
        {
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

        var hosted_counter = 0;
        List<Event> countEvent = await _eventService.GetByUserId(proUser.Id);
        foreach(var cE in countEvent)
        {hosted_counter++;}

        List<Participant> countParti = await _participantService.GetByUserId(proUser.Id);
        var participated_counter = 0;
        foreach(var cP in countParti)
        {
            var Event = await _eventService.GetById(cP.event_id);
            if (Event != null)
            {
            if(Event.status == false && Event != null)
            {participated_counter++;}
            }
        }

        ViewBag.first = unow.firstname;
        ViewBag.last = unow.lastname;
        ViewBag.mail = unow.email;
        ViewBag.image = unow.profile_img;
        ViewBag.description = unow.description;
        ViewBag.participated_counter = participated_counter;
        ViewBag.Hosted_evented = hosted_counter;
        ViewBag.ShortEventDisplay = allEvent;
        return View(proUser);
    }
        
    }
}
