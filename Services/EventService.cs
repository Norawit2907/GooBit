using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;

namespace GooBitAPI.Services
{
    public class EventService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<Event> _eventCollection;
        private readonly IConfiguration _configuration;
        public EventService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBservice = mongoDBService;
            _eventCollection = _mongoDBservice._eventCollection;
            _configuration = configuration;
        }
        public async Task<List<Event>> GetAsync() =>
            await _eventCollection.Find(_ => true).ToListAsync();

        public async Task<Event?> GetById(string id) =>
            await _eventCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Event>> GetByCategory(string category)
        {
            List<Event> _events = await _eventCollection.Find(x => x.category == category).ToListAsync();
            return _events;
        }

        public ShortEventDisplay MakeSEvent(Event _event, string firstname, string lastname, string user_image)
        {
            ShortEventDisplay ShEvD = new ShortEventDisplay{
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
                firstname = firstname,
                lastname = lastname,
                user_image = user_image,
                latitude = _event.latitude,
                longitude = _event.longitude
            };
            return ShEvD;
        }

        public EventDisplay MakeEventDisplay(Event _event, User _user , List<ShowComment> _comments, List<Participant> _participants)
        {
            EventDisplay ED = new EventDisplay{
                Id = _event.Id,
                creator_Id = _event.user_id,
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
                firstname = _user.firstname,
                lastname = _user.lastname,
                user_image = _user.profile_img,
                latitude = _event.latitude,
                longitude = _event.longitude,
                comments = _comments,
                participants = _participants,
            };
            return ED;
        }

        public async Task<List<ShortEventDisplay>> GetByCreateUser(string id, string firstname, string lastname, string user_image)
        {
            List<Event> _events = await _eventCollection.Find(x => x.user_id == id).ToListAsync();
            List<ShortEventDisplay> sEvent = new List<ShortEventDisplay>();
            foreach (Event _event in _events)
            {
                sEvent.Add(MakeSEvent(_event,firstname,lastname, user_image));
            }
            return sEvent;
        }

        public async Task<List<Event>> UpdateCloseEvent()
        {
            List<Event> _events = await _eventCollection.Find(_ => true).ToListAsync();
            List<Event> closedEvents = [];
            foreach (Event _event in _events)
            {
                if (_event.end_date.CompareTo(DateTime.UtcNow) <= 0 && _event.status)
                {
                    _event.status = false;
                    if (_event.Id != null)
                    {
                        await _eventCollection.ReplaceOneAsync(x => x.Id == _event.Id, _event);
                    }
                    closedEvents.Add(_event);
                }
            }
            return closedEvents;
        }

        public async Task CreateAsync(Event newEvent) =>
            await _eventCollection.InsertOneAsync(newEvent);

        public async Task UpdateAsync(string id, Event updatedEvent) =>
            await _eventCollection.ReplaceOneAsync(x => x.Id == id, updatedEvent);

        public async Task RemoveAsync(string id) =>
            await _eventCollection.DeleteOneAsync(x => x.Id == id);
    }
}