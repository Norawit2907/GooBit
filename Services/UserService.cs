using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;

namespace GooBitAPI.Services
{
    public class UserService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<User> _userCollection;
        private readonly IConfiguration _configuration;
        public UserService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBservice = mongoDBService;
            _userCollection = _mongoDBservice._userCollection;
            _configuration = configuration;
        }

        public async Task<List<User>> GetAsync() =>
            await _userCollection.Find(_ => true).ToListAsync();

        public async Task<User?> GetAsync(string id) =>
            await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<string?> Login(Login login)
        {
            var user = await _userCollection.Find(x => x.email == login.email).SingleOrDefaultAsync();
            if (user == null)
            {
                return null;
            }
            if (user.password != login.password || user.Id == null)
            {
                return null;
            }
            return user.Id;
        }

        public async Task CreateAsync(User newUser) =>
            await _userCollection.InsertOneAsync(newUser);

        public async Task<UserNoPassword> UpdateAsync(string id, UpdateUser updatedUser)
        {
            User userdata = await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            var _firstname = updatedUser.firstname != null? updatedUser.firstname:userdata.firstname;
            var _lastname = updatedUser.lastname != null? updatedUser.lastname:userdata.lastname;
            var _email = updatedUser.email != null? updatedUser.email:userdata.email;
            var _password = updatedUser.password != null? updatedUser.password:userdata.password;
            var _profile_img = updatedUser.profile_img != null? updatedUser.profile_img:userdata.profile_img;
            var _description = updatedUser.description != null? updatedUser.description:userdata.description;
            var _update = Builders<User>.Update.Set("firstname",_firstname)
                                                .Set("lastname",_lastname)
                                                .Set("email",_email)
                                                .Set("password",_password)
                                                .Set("profile_img",_profile_img)
                                                .Set("description",_description);
            await _userCollection.FindOneAndUpdateAsync(_user => _user.Id == id, _update);
            User user = await _userCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
            var userNoPW = new UserNoPassword{
                username = user.username,
                email = user.email,
                firstname = user.firstname,
                lastname = user.lastname,
                description = user.description,
                profile_img = user.profile_img
            };
            return userNoPW;
        }

        public async Task RemoveAsync(string id) =>
            await _userCollection.DeleteOneAsync(x => x.Id == id);
    }
}


