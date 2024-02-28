using GooBitAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GooBitAPI.Services
{
    public class MongoDBService
    {
        public readonly IMongoCollection<Book> _bookCollection;
        public readonly IMongoCollection<User> _userCollection;

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _userCollection = database.GetCollection<User>("Users");
            _bookCollection = database.GetCollection<Book>("Books");

        }

    }
}