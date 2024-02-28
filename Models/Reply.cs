using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GooBitAPI.Models
{
    public class Reply
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string user_id { get; set; } = null!;
        public string comment_id {get; set;} = null!;
        public string text {get; set;} = null!;

    }
}