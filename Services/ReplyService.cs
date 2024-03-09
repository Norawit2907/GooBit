using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;

namespace GooBitAPI.Services
{
    public class ReplyService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<Reply> _replyCollection;
        private readonly IConfiguration _configuration;
        public ReplyService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBservice = mongoDBService;
            _replyCollection = _mongoDBservice._replyCollection;
            _configuration = configuration;
        }

        public async Task<List<Reply>> GetAsync() =>
            await _replyCollection.Find(_ => true).ToListAsync();

        public async Task<Reply?> GetAsyncById(string id) =>
            await _replyCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Reply>> GetRepliesAsyncByCommentId(string id) =>
            await _replyCollection.Find(x => x.comment_id == id).ToListAsync();

        public async Task CreateAsync(Reply newReply) =>
            await _replyCollection.InsertOneAsync(newReply);

        public async Task UpdateAsync(string id, Reply updatedReply) =>
            await _replyCollection.ReplaceOneAsync(x => x.Id == id, updatedReply);

        public async Task RemoveAsync(string id) =>
            await _replyCollection.DeleteOneAsync(x => x.Id == id);
    }
}


