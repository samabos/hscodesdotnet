using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autumn.Domain.Models;
using MongoDB.Driver;

namespace Autumn.Domain.Services
{
   
    public class SearchLogService
    {
        private readonly IMongoCollection<SearchLog> _row;

        public SearchLogService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _row = database.GetCollection<SearchLog>(settings.SearchLogStoreCollectionName);
        }

        public List<SearchLog> Get() =>
            _row.Find<SearchLog>(x => true).ToList();
        public async Task<List<SearchLog>> GetAsync() =>
             await _row.Find<SearchLog>(x => true).ToListAsync();

        public SearchLog Get(string id) =>
            _row.Find<SearchLog>(x => x.Id == id).FirstOrDefault();

        public SearchLog Create(SearchLog x)
        {
            _row.InsertOne(x);
            return x;
        }

        public void Update(string id, SearchLog xIn) =>
            _row.ReplaceOne(x => x.Id == id, xIn);

        public void Remove(SearchLog xIn) =>
            _row.DeleteOne(x => x.Id == xIn.Id);

        public void Remove(string id) =>
            _row.DeleteOne(x => x.Id == id);
    }
}
