using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;

namespace GooBitAPI.Services
{
    public class ParticipantService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<Participant> _participantCollection;
        private readonly IConfiguration _configuration;
        public ParticipantService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBservice = mongoDBService;
            _participantCollection = _mongoDBservice._participantCollection;
            _configuration = configuration;
        }

        public async Task<List<Participant>> GetAsync() =>
            await _participantCollection.Find(_ => true).ToListAsync();

        public async Task<Participant?> GetAsync(string id) =>
            await _participantCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Participant>> GetByUser(string id) =>
            await _participantCollection.Find(x => x.user_id == id).ToListAsync();
        

        public async Task CreateAsync(Participant newParticipant) =>
            await _participantCollection.InsertOneAsync(newParticipant);

        public async Task UpdateAsync(string id, Participant updatedParticipant) =>
            await _participantCollection.ReplaceOneAsync(x => x.Id == id, updatedParticipant);

        public async Task RemoveAsync(string id) =>
            await _participantCollection.DeleteOneAsync(x => x.Id == id);
    }
}


