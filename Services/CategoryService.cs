using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;

namespace GooBitAPI.Services
{
    public class CategoryService
    {
        private MongoDBService _mongoDBservice;
        private IMongoCollection<Category> _categoryCollection;
        private readonly IConfiguration _configuration;
        public CategoryService(MongoDBService mongoDBService, IConfiguration configuration)
        {
            _mongoDBservice = mongoDBService;
            _categoryCollection = _mongoDBservice._categoryCollection;
            _configuration = configuration;
        }

        public async Task<List<Category>> GetAsync() =>
            await _categoryCollection.Find(_ => true).ToListAsync();

        public async Task<Category?> GetAsync(string id) =>
            await _categoryCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Category newCategory) =>
            await _categoryCollection.InsertOneAsync(newCategory);

        public async Task UpdateAsync(string id, Category updatedCategory) =>
            await _categoryCollection.ReplaceOneAsync(x => x.Id == id, updatedCategory);

        public async Task RemoveAsync(string id) =>
            await _categoryCollection.DeleteOneAsync(x => x.Id == id);
    }
}


