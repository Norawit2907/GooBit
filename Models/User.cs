using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GooBitAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string username { get; set; } = null!;
        public string password { get; set; } = null!;
        public string email { get; set; } = null!;
        public string? description { get; set; } = null!;
        public string? firstname { get; set; } = null!;
        public string? lastname { get; set; } = null!;
        public string? profile_img { get; set; } = null!;

    }
}