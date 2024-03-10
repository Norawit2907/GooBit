using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GooBitAPI.Models
{
    public class Notification
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string user_id { get; set; } = null!;
        public string event_id {get; set;} = null!;
        public string body {get; set;} = null!;
    }
}