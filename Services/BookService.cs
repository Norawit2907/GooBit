using MongoDB.Driver;
using MongoDB.Bson;
using GooBitAPI.Models;
using Microsoft.Extensions.Options;

namespace GooBitAPI.Services
{
    public class BooksService
    {
            private MongoDBService _mongoDBservice;
            private IMongoCollection<Book> _booksCollection;
            private readonly IConfiguration _configuration;
            public BooksService(MongoDBService mongoDBService, IConfiguration configuration)
            {
                _mongoDBservice = mongoDBService;
                _booksCollection = _mongoDBservice._bookCollection;
                _configuration = configuration;
            }

        public async Task<List<Book>> GetAsync() =>
            await _booksCollection.Find(_ => true).ToListAsync();

        public async Task<Book?> GetAsync(string id) =>
            await _booksCollection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Book newBook) =>
            await _booksCollection.InsertOneAsync(newBook);

        public async Task UpdateAsync(string id, Book updatedBook) =>
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);

        public async Task RemoveAsync(string id) =>
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
    }
}