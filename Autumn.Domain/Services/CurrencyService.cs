using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Autumn.Domain.Services
{

    public class CurrencyService
    {
        private readonly IMongoCollection<Currency> _object;

        public CurrencyService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _object = database.GetCollection<Currency>(settings.CurrencyStoreCollectionName);
        }
        public List<Currency> Get()
        {
            return _object
                .Find<Currency>(x => true) // Fetch all documents
                .SortByDescending(x => x.TimeStamp) // Sort by the timestamp field in descending order
                .Limit(12) // Limit the result to the 12 most recent documents
                .ToList(); // Convert the result to a list
        }

        public async Task<List<Currency>> GetAsync() =>
             await _object.Find<Currency>(x => true).ToListAsync();

        public Currency Get(string id) =>
            _object.Find<Currency>(x => x.Id == id).FirstOrDefault();
        public Currency GetByCurrency(string currency) =>
            _object.Find<Currency>(x => x.CurrencyCode == currency).FirstOrDefault();

        public Currency Create(Currency x)
        {
            _object.InsertOne(x);
            return x;
        }

        public void Update(string id, Currency xIn) =>
            _object.ReplaceOne(x => x.Id == id, xIn);

        public void Remove(Currency xIn) =>
            _object.DeleteOne(x => x.Id == xIn.Id);

        public void Remove(string id) =>
            _object.DeleteOne(x => x.Id == id);
    }
}
