using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GooBitAPI.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string title {get; set;} = null!;
        public string description {get; set;} = null!;
        public int total_member {get; set;} = 0;
        public int max_member {get; set;} = 0!;
        public DateTime end_date {get; set;}
        public DateTime event_date {get; set;}
        public string duration {get; set;} = null!;
        public string? googlemap_location {get; set;} = null!;
        public List<string> event_img {get; set;} = [];
        public string? category {get; set;} = null!;
        public bool status {get; set;} = true;
        public string? user_id {get; set;} = null!;
        public decimal? latitude {get; set;} = 0!;
        public decimal? longitude {get; set;} = 0!;
    }

    public class ShortEventDisplay
    {

        public string? Id { get; set; }
        public string title {get; set;} = null!;
        public string description {get; set;} = null!;
        public int total_member {get; set;} = 0;
        public int max_member {get; set;} = 0!;
        public DateTime end_date {get; set;}
        public DateTime event_date {get; set;}
        public int duration {get; set;} = 0!;
        public string? googlemap_location {get; set;} = null!;
        public List<string> event_img {get; set;} = null!;
        public string category {get; set;} = null!;
        public bool status {get; set;} = true;
        public string firstname {get; set;} = null!;
        public string lastname {get; set;} = null!;
        public decimal? latitude {get; set;} = 0!;
        public decimal? longtitude {get; set;} = 0!;
    }

}