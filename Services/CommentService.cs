using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;

namespace GooBitAPI.Services
{
    public class CommentService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<Comment> _commentCollection;
        private readonly IConfiguration _configuration;
        public CommentService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBservice = mongoDBService;
            _commentCollection = _mongoDBservice._commentCollection;
            _configuration = configuration;
        }

        public async Task<List<Comment>> GetAsync() =>
            await _commentCollection.Find(_ => true).ToListAsync();

        public async Task<Comment?> GetAsync(string id) =>
            await _commentCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
        public async Task<List<Comment>?> GetByEventId(string event_id) =>
            await _commentCollection.Find(x => x.event_id == event_id).ToListAsync();

        public ShowComment MakeSComment(Comment _comment, string firstname, string lastname, List<Reply> _replies)
        {
            ShowComment SC = new ShowComment{
                Id = _comment.Id,
                user_id = _comment.user_id,
                user_image = _comment.user_image,
                firstname = firstname,
                lastname = lastname,
                event_id = _comment.event_id,
                text = _comment.text,
                create_time = _comment.create_time,
                replies = _replies
            };
            return SC;
        }

        public async Task CreateAsync(Comment newComment) =>
            await _commentCollection.InsertOneAsync(newComment);

        public async Task UpdateAsync(string id, Comment updatedComment) =>
            await _commentCollection.ReplaceOneAsync(x => x.Id == id, updatedComment);

        public async Task RemoveAsync(string id) =>
            await _commentCollection.DeleteOneAsync(x => x.Id == id);
        
    }
}

