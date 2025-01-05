using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Services
{

    public class ProductService2
    {
        private readonly IMongoCollection<Product2> _product;

        public ProductService2(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _product = database.GetCollection<Product2>(settings.Product2StoreCollectionName);
        }

        #region List
        public List<Product2> Get() =>
            _product.Find<Product2>(x => true).ToList();
        public async Task<List<Product2>> GetAsync() =>
             await _product.Find<Product2>(x => true).ToListAsync();

        public Product2 Get(string id) =>
            _product.Find<Product2>(x => x.Id == id).FirstOrDefault();
        public async Task<Product2> GetAsync(string id) =>
           await _product.Find<Product2>(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Product2>> GetByTagsAsync(string tag)
        {
            List<Product2> t = new List<Product2>();
            if (string.IsNullOrEmpty(tag))
                t = await _product.Find<Product2>(x => x.Tags != null).ToListAsync();
            else
                t = await _product.Find<Product2>(x => x.Tags.Contains(tag)).ToListAsync();
            return t;
        }
        public List<Product2> GetByKeyword(string keyword) =>
         _product.Find<Product2>(x => x.Keyword == keyword).ToList();

        public async Task<List<Product2>> GetByKeywordAsync(string keyword) =>
         await _product.Find<Product2>(x => x.Keyword.ToLower() == keyword.ToLower()).ToListAsync();

        public async Task<List<Product2>> GetLikeKeywordAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {

                return await _product.Find<Product2>(x => true).ToListAsync();
            }
            else
            {
                return await _product.Find<Product2>(x => x.Keyword.ToLower().StartsWith(keyword.ToLower())).ToListAsync();
            }
        }
        #endregion



        public Product2 Create(Product2 x)
        {
            var now = DateTime.Now;
            x.Created = now;
            x.Modified = now;
            _product.InsertOne(x);
            return x;
        }

        public void Update(string id, Product2 xIn)
        {
            var now = DateTime.Now;
            xIn.Modified = now;
            _product.ReplaceOne(x => x.Id == id, xIn);
        }
        public void Remove(Product xIn) =>
            _product.DeleteOne(x => x.Id == xIn.Id);

        public void Remove(string id) =>
            _product.DeleteOne(x => x.Id == id);
    }
}





