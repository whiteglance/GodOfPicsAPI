using GodOfPicsAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Core.Operations;
using MongoDB.Driver.Core.Configuration;

namespace GodOfPicsAPI.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Photos> _photosCollection;


        /*"ConnectionString": "mongodb+srv://user:user@godofpics.qbrenou.mongodb.net/?retryWrites=true&w=majority",
        "DatabaseName": "mongo",
        "CollectionName": "Photos"*/

        public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionString);
            IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _photosCollection = database.GetCollection<Photos>(mongoDBSettings.Value.CollectionName);
        }  
        
        public async Task CreateAsync(Photos photo)
        {
            await _photosCollection.InsertOneAsync(photo);
        }

        public async Task<List<Photos>> GetAsync()
        {
            return await _photosCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Photos> DeleteByIdAsync(string id)
        {
            var filter = Builders<Photos>.Filter.Eq(p => p.Id, id);
            return await _photosCollection.FindOneAndDeleteAsync(filter);
        }

        public async Task AddToPhotosAsync(string id, string photoId)
        {
            FilterDefinition<Photos> filter = Builders<Photos>.Filter.Eq("Id", id);
            UpdateDefinition<Photos> update = Builders<Photos>.Update.AddToSet<string>("photoId", photoId);
            await _photosCollection.UpdateOneAsync(filter, update);
            return;
        }
    }
}
