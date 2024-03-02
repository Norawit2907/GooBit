using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GooBitAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [Required(ErrorMessage = "Please enter username")]
        public string username { get; set; } = null!;
        [Required(ErrorMessage = "Please enter password")]
        public string password { get; set; } = null!;
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress]
        public string email { get; set; } = null!;
        [Required(ErrorMessage = "Please enter first name")]
        public string firstname { get; set; } = null!;
        [Required(ErrorMessage = "Please enter last name")]
        public string lastname { get; set; } = null!;
        public string? description { get; set; } = null!;
        public string? profile_img { get; set; } = null!;

    }

    public class Login
    {
        [Required(ErrorMessage = "Please enter email")]
        [EmailAddress]
        public string email { get; set; } = null!;
        [Required(ErrorMessage = "Please enter password")]
        public string password { get; set;} = null!;
    }
}