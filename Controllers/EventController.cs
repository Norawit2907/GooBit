using GooBitAPI.Models;
using GooBitAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GooBitAPI.Controllers;

public class EventController : Controller
{
    private readonly EventService _eventService;
    public EventController(EventService eventService) =>
        _eventService = eventService;
}