using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GooBitAPI.Models
{
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("Name")]
        public string BookName { get; set; } = null!;
        public int Price { get; set; }
        public string Category { get; set; } = null!;
        public string Author { get; set; } = null!;
    }
}