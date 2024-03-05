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
        [Required(ErrorMessage = "Please enter username.")]
        public string username { get; set; } = null!;
        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        public string password { get; set; } = null!;
        [Required(ErrorMessage = "Please enter email.")]
        [EmailAddress(ErrorMessage = "Please enter email.")]
        public string email { get; set; } = null!;
        [Required(ErrorMessage = "Please enter first name.")]
        public string firstname { get; set; } = null!;
        [Required(ErrorMessage = "Please enter last name.")]
        public string lastname { get; set; } = null!;
        public string? description { get; set; } = null!;
        public string? profile_img { get; set; } = null!;

    }

    public class Login
    {
        [Required(ErrorMessage = "Please enter email.")]
        [EmailAddress]
        public string email { get; set; } = null!;
        [Required(ErrorMessage = "Please enter password.")]
        [DataType(DataType.Password)]
        public string password { get; set;} = null!;
    }

    public class UpdateUser
    {
        public string? firstname { get; set; }
        public string? lastname { get; set; }
        [EmailAddress]
        public string? email { get; set; }
        [DataType(DataType.Password)]
        public string? password {get; set; }
        [DataType(DataType.Password)]
        [Compare("password",ErrorMessage = "Password did not match.")]
        public string? confirm_password { get; set; }
        public string? profile_img { get; set; }
        public string? description { get; set; } 
    }

    public class UserNoPassword
    {
        public string username { get; set; } = null!;
        [EmailAddress]
        public string email { get; set; } = null!;
        public string firstname { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public string? description { get; set; } = null!;
        public string? profile_img { get; set; } = null!;
    }

    public class UserProfile
    {   
        public string email { get; set; } = null!;
        public string firstname { get; set; } = null!;
        public string lastname { get; set; } = null!;
        public string? description { get; set; } = null!;
        public string? profile_img { get; set; } = null!;
        public List<ShortEventDisplay> owned_event { get; set;} = null!;
        public List<ShortEventDisplay> joined_event { get; set;} = null!;
    }

}