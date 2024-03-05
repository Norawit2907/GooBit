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

        public async Task<Event?> GetAsync(string id) =>
            await _eventCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Event newEvent) =>
            await _eventCollection.InsertOneAsync(newEvent);

        public async Task UpdateAsync(string id, Event updatedEvent) =>
            await _eventCollection.ReplaceOneAsync(x => x.Id == id, updatedEvent);

        public async Task RemoveAsync(string id) =>
            await _eventCollection.DeleteOneAsync(x => x.Id == id);
    }
}