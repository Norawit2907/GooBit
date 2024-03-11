using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;
using System.Drawing;

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

        public async Task<Notification?> GetAsyncById(string id) =>
            await _notificationCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Notification>?> GetAsyncByUserId(string id) =>
            await _notificationCollection.Find(x => x.user_id == id).ToListAsync();

        public async Task CreateAsync(Notification newNotification) =>
            await _notificationCollection.InsertOneAsync(newNotification);

        public async Task CreateNoti(string user_id, string event_id, string body)
        {
            Notification noti = new Notification{
                user_id = user_id,
                event_id = event_id,
                body = body,
            };
            await _notificationCollection.InsertOneAsync(noti);
        }

        public ShowNotification MakeSNotification(Notification _noti, string firstname, string lastname, string event_title, string user_image)
        {
            ShowNotification SNT = new ShowNotification{
                Id = _noti.Id,
                user_id = _noti.user_id,
                user_image = user_image,
                event_id = _noti.event_id,
                body = _noti.body,
                event_title = event_title,
                firstname = firstname,
                lastname = lastname
            };
            return SNT;
        }

        public async Task UpdateAsync(string id, Notification updatedNotification) =>
            await _notificationCollection.ReplaceOneAsync(x => x.Id == id, updatedNotification);

        public async Task RemoveAsync(string id) =>
            await _notificationCollection.DeleteOneAsync(x => x.Id == id);
    }
}


