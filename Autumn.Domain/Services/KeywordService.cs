using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Services
{

    public class KeywordService
    {
        private readonly IMongoCollection<Keyword> _keyword;

        public KeywordService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _keyword = database.GetCollection<Keyword>(settings.KeywordStoreCollectionName);
        }

        public List<Keyword> Get() =>
            _keyword.Find<Keyword>(x => true).ToList();
        public async Task<List<Keyword>> GetAsync() =>
             await _keyword.Find<Keyword>(x => true).ToListAsync();
        
        public Keyword Get(string id) =>
            _keyword.Find<Keyword>(x => x.Id == id).FirstOrDefault();

        public Keyword Create(Keyword x)
        {
            _keyword.InsertOne(x);
            return x;
        }

        public void Update(string id, Keyword xIn) =>
            _keyword.ReplaceOne(x => x.Id == id, xIn);

        public void Remove(Keyword xIn) =>
            _keyword.DeleteOne(x => x.Id == xIn.Id);

        public void Remove(string id) =>
            _keyword.DeleteOne(x => x.Id == id);
    }
}
