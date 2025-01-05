using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Services
{

    public class CustomsTariffService
    {
        private readonly IMongoCollection<CustomsTariff> _object;

        public CustomsTariffService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _object = database.GetCollection<CustomsTariff>(settings.CustomsTariffStoreCollectionName);
        }

        public List<CustomsTariff> Get() =>
            _object.Find<CustomsTariff>(x => true).ToList();
        public async Task<List<CustomsTariff>> GetAsync() =>
             await _object.Find<CustomsTariff>(x => true).ToListAsync();

        public CustomsTariff Get(string id) =>
            _object.Find<CustomsTariff>(x => x.Id == id).FirstOrDefault();
        public async Task<CustomsTariff> GetAsync(string id) =>
           await _object.Find<CustomsTariff>(x => x.Id == id).FirstOrDefaultAsync();


        public CustomsTariff GetByHSCode(string hscode) =>
            _object.Find<CustomsTariff>(x => x.HSCode == hscode).FirstOrDefault();
        public List<CustomsTariff> GetByHeader(string header) =>
            _object.Find<CustomsTariff>(x => x.Header == header).ToList();
        public async Task<List<CustomsTariff>> GetByHeaderAsync(string header) =>
                 await _object.Find<CustomsTariff>(x => x.Header == header).ToListAsync();

        public CustomsTariff Create(CustomsTariff x)
        {
            _object.InsertOne(x);
            return x;
        }

        public void Update(string id, CustomsTariff xIn) =>
            _object.ReplaceOne(x => x.Id == id, xIn);

        public void Remove(CustomsTariff xIn) =>
            _object.DeleteOne(x => x.Id == xIn.Id);

        public void Remove(string id) =>
            _object.DeleteOne(x => x.Id == id);
    }
}
