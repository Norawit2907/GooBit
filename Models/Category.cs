using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GooBitAPI.Models
{
    public class Category
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string body { get; set; } = null!;

    }
}