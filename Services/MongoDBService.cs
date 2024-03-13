using GooBitAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GooBitAPI.Services
{
    public class MongoDBService
    {
        public readonly IMongoCollection<User> _userCollection;
        public readonly IMongoCollection<Event> _eventCollection;
        public readonly IMongoCollection<Category> _categoryCollection;
        public readonly IMongoCollection<Comment> _commentCollection;
        public readonly IMongoCollection<Notification> _notificationCollection;
        public readonly IMongoCollection<Participant> _participantCollection;
        public readonly IMongoCollection<Reply> _replyCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>("Users");
            _eventCollection = database.GetCollection<Event>("Events");
            _categoryCollection = database.GetCollection<Category>("Categorys");
            _commentCollection = database.GetCollection<Comment>("Comments");
            _notificationCollection = database.GetCollection<Notification>("Notifications");
            _participantCollection = database.GetCollection<Participant>("Participants");
            _replyCollection = database.GetCollection<Reply>("Replies");

        }

    }
}