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

        public ShortEventDisplay MakeSEvent(Event _event, string firstname, string lastname)
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
                latitude = _event.latitude,
                longitude = _event.longitude
            };
            return ShEvD;
        }

        public async Task<List<ShortEventDisplay>> GetByCreateUser(string id, string firstname, string lastname)
        {
            List<Event> _events = await _eventCollection.Find(x => x.user_id == id).ToListAsync();
            List<ShortEventDisplay> sEvent = new List<ShortEventDisplay>();
            foreach (Event _event in _events)
            {
                sEvent.Add(MakeSEvent(_event,firstname,lastname));
            }
            return sEvent;
        }

        public async Task CreateAsync(Event newEvent) =>
            await _eventCollection.InsertOneAsync(newEvent);

        public async Task UpdateAsync(string id, Event updatedEvent) =>
            await _eventCollection.ReplaceOneAsync(x => x.Id == id, updatedEvent);

        public async Task RemoveAsync(string id) =>
            await _eventCollection.DeleteOneAsync(x => x.Id == id);
    }
}