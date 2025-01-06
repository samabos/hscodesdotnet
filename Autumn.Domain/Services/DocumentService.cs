using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Services
{

    public class DocumentService
    {
        private readonly IMongoCollection<Document> _object;

        public DocumentService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _object = database.GetCollection<Document>(settings.DocumentStoreCollectionName);
        }

        public List<Document> Get() =>
            _object.Find<Document>(x => true).ToList();
        public async Task<List<Document>> GetAsync() =>
             await _object.Find<Document>(x => true).ToListAsync();
        
        public Document Get(string id) =>
            _object.Find<Document>(x => x.Id == id).FirstOrDefault();

        public Document Create(Document x)
        {
            _object.InsertOne(x);
            return x;
        }

        public void Update(string id, Document xIn) =>
            _object.ReplaceOne(x => x.Id == id, xIn);

        public void Remove(Document xIn) =>
            _object.DeleteOne(x => x.Id == xIn.Id);

        public void Remove(string id) =>
            _object.DeleteOne(x => x.Id == id);
    }
}
