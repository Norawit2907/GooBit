using GooBitAPI.Models;
using GooBitAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace GooBitAPI.Controllers;

public class ParticipantController: Controller
{
    private readonly ParticipantService _participantService;

    public ParticipantController(ParticipantService participantService)
    {
        _participantService = participantService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Participant>>> GetByEventId(string id)
    {
        List<Participant>? _participants = await _participantService.GetByEventId(id);
        return _participants;
    }
}