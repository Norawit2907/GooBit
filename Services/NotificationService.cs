using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;

namespace GooBitAPI.Services
{
    public class NotificationService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<Notification> _notificationCollection;
        private readonly IConfiguration _configuration;
        public NotificationService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBservice = mongoDBService;
            _notificationCollection = _mongoDBservice._notificationCollection;
            _configuration = configuration;
        }

        public async Task<List<Notification>> GetAsync() =>
            await _notificationCollection.Find(_ => true).ToListAsync();

        public async Task<Notification?> GetAsync(string id) =>
            await _notificationCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Notification newNotification) =>
            await _notificationCollection.InsertOneAsync(newNotification);

        public async Task UpdateAsync(string id, Notification updatedNotification) =>
            await _notificationCollection.ReplaceOneAsync(x => x.Id == id, updatedNotification);

        public async Task RemoveAsync(string id) =>
            await _notificationCollection.DeleteOneAsync(x => x.Id == id);
    }
}


