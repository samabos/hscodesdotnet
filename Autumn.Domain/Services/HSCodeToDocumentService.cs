using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Services
{

    public class HSCodeToDocumentService
    {
        private readonly IMongoCollection<HSCodeToDocument> _object;

        public HSCodeToDocumentService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _object = database.GetCollection<HSCodeToDocument>(settings.HSCodeToDocumentStoreCollectionName);
        }

        public List<HSCodeToDocument> Get() =>
            _object.Find<HSCodeToDocument>(x => true).ToList();
        public async Task<List<HSCodeToDocument>> GetAsync() =>
             await _object.Find<HSCodeToDocument>(x => true).ToListAsync();
        
        public HSCodeToDocument Get(string id) =>
            _object.Find<HSCodeToDocument>(x => x.Id == id).FirstOrDefault();
        public async Task<List<HSCodeToDocument>> GetWithCodeAsync(string code) =>
          await _object.Find<HSCodeToDocument>(x => x.Hscode == code).ToListAsync();


        public HSCodeToDocument Create(HSCodeToDocument x)
        {
            _object.InsertOne(x);
            return x;
        }

        public void Update(string id, HSCodeToDocument xIn) =>
            _object.ReplaceOne(x => x.Id == id, xIn);

        public void Remove(HSCodeToDocument xIn) =>
            _object.DeleteOne(x => x.Id == xIn.Id);

        public void Remove(string id) =>
            _object.DeleteOne(x => x.Id == id);
    }
}
