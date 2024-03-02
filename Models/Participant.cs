using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GooBitAPI.Models
{
    public class Participant
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string event_id { get; set; } = null!;
        public string user_id {get; set;} = null!;
        public string status {get; set;} = null!;

    }
}