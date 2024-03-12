using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;
using System.Data;

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

        public async Task<Participant?> GetAsyncById(string id) =>
            await _participantCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Participant>> GetByEventId(string id) =>
            await _participantCollection.Find(x => x.event_id == id).ToListAsync();

        public async Task<List<Participant>> GetByUserId(string id) =>
            await _participantCollection.Find(x => x.user_id == id).ToListAsync();

        public async Task<Participant?> GetByEU(string user_id, string event_id)
        {
            var filterBuilder = Builders<Participant>.Filter;
            var filter = filterBuilder.And(
                filterBuilder.Eq("user_id", user_id),
                filterBuilder.Eq("event_id", event_id)
            );
            Participant? participant = await _participantCollection.Find(filter).FirstOrDefaultAsync();
            return participant;
        }

        public async Task<int> statusChanger(string event_id, string? prevUserS, string? UserS)
        {
            List<string> prevUserList = prevUserS == null? []:prevUserS.Split(",").ToList();
            List<string> UserList = UserS == null? []:UserS.Split(",").ToList();
            int rejected_user = 0;
            foreach(string prev_user in prevUserList)
            {
                if (UserList.Contains(prev_user))
                {
                    UserList.Remove(prev_user);
                } else
                {
                    Participant? participant = await GetByEU(prev_user,event_id);
                    if (participant != null && participant.Id != null)
                    {
                        participant.status = "rejected";
                        await UpdateAsync(participant.Id,participant);
                        rejected_user ++;
                    }
                }
            }
            foreach(string user in UserList)
            {
                Participant? participant = await GetByEU(user,event_id);
                if (participant != null && participant.Id != null)
                {
                    if (participant.status == "rejected"){rejected_user --;}
                    participant.status = "submitted";
                    await UpdateAsync(participant.Id,participant);
                }
            }
            return rejected_user;
        }

        public ShowParticipant MakeShowParticipant(Participant _participant, string firstname, string lastname, string user_image)
        {
            ShowParticipant SP = new ShowParticipant{
                Id = _participant.Id,
                event_id = _participant.event_id,
                user_id = _participant.user_id,
                firstname = firstname,
                lastname = lastname,
                user_image = user_image,
                status = _participant.status
            };
            return SP;
        }
        public async Task CreateAsync(Participant newParticipant) =>
            await _participantCollection.InsertOneAsync(newParticipant);

        public async Task UpdateAsync(string id, Participant updatedParticipant) =>
            await _participantCollection.ReplaceOneAsync(x => x.Id == id, updatedParticipant);

        public async Task RemoveAsync(string id) =>
            await _participantCollection.DeleteOneAsync(x => x.Id == id);
    }
}