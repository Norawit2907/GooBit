using System.Globalization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace GooBitAPI.Models
{
    public class Event
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [Required(ErrorMessage = "Please enter title.")]
        public string title { get; set; } = null!;
        [Required(ErrorMessage = "Please enter description.")]
        public string description { get; set; } = null!;
        public int total_member { get; set; } = 0;
        [Required(ErrorMessage = "Please enter member number.")]
        public int max_member { get; set; } = 0!;
        [Required(ErrorMessage = "Please enter Date.")]
        public DateTime end_date { get; set; }
        [Required(ErrorMessage = "Please enter Date.")]
        public DateTime event_date { get; set; }
        [Required(ErrorMessage = "Please enter event duration.")]
        public string duration { get; set; } = null!;
        public string? googlemap_location { get; set; } = null!;
        public List<string> event_img { get; set; } = [];
        public string category { get; set; } = null!;
        public bool status { get; set; } = true;
        public string user_id { get; set; } = null!;
        public decimal? latitude { get; set; } = 0!;
        public decimal? longitude { get; set; } = 0!;
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
        public string duration {get; set;} = null!;
        public string? googlemap_location {get; set;} = null!;
        public List<string> event_img {get; set;} = null!;
        public string category {get; set;} = null!;
        public bool status {get; set;} = true;
        public string firstname {get; set;} = null!;
        public string lastname {get; set;} = null!;
        public string user_image {get; set;} = "default_img.png";
        public decimal? latitude {get; set;} = 0!;
        public decimal? longitude {get; set;} = 0!;
    }

    public class EventDisplay
    {
        public string? Id { get; set; }
        public string title {get; set;} = null!;
        public string description {get; set;} = null!;
        public int total_member {get; set;} = 0;
        public int max_member {get; set;} = 0!;
        public DateTime end_date {get; set;}
        public DateTime event_date {get; set;}
        public string duration {get; set;} = null!;
        public string? googlemap_location {get; set;} = null!;
        public List<string> event_img {get; set;} = null!;
        public string category {get; set;} = null!;
        public bool status {get; set;} = true;
        public string firstname {get; set;} = null!;
        public string lastname {get; set;} = null!;
        public string? user_image {get; set;} = null!;
        public decimal? latitude {get; set;} = 0!;
        public decimal? longitude {get; set;} = 0!;
        public List<Comment> comments {get; set;} = [];
        public List<Participant> participants {get; set;} = [];
        public List<Reply> replies {get; set;} = [];
    }

    public class EditEventDisplay
    {
        public string? Id { get; set; } = null!;
        public string title { get; set; } = null!;
        public string description { get; set; } = null!;
        public int total_member { get; set; } = 0;
        public int max_member { get; set; } = 0!;
        public DateTime end_date { get; set; }
        public DateTime event_date { get; set; }
        public string duration { get; set; } = null!;
        public string? googlemap_location { get; set; } = null!;
        public List<string> event_img { get; set; } = [];
        public string category { get; set; } = null!;
        public bool status { get; set; } = true;
        public string? user_id { get; set; } = null!;
        public decimal? latitude { get; set; } = 0!;
        public decimal? longitude { get; set; } = 0!;
        public int available_user { get; set; } = 0;
        public List<UserStatus> submitted_user { get; set; } = [];
        public List<UserStatus> participants { get; set; } = [];
    }

    public class UpdatedEvent
    {
        public string title { get; set; } = null!;
        public string description { get; set; } = null!;
        public int max_member { get; set; } = 0!;
        public DateTime end_date { get; set; }
        public DateTime event_date { get; set; }
        public string duration { get; set; } = null!;
        public string? googlemap_location { get; set; } = null!;
        public string category { get; set; } = null!;
        public string status { get; set; } = null!;
        public decimal? latitude { get; set; } = 0!;
        public decimal? longitude { get; set; } = 0!;
        public string? submitted_user { get; set; }
    }

}