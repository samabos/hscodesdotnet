using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Services
{

    public class ProductService
    {
        private readonly IMongoCollection<Product> _product;

        public ProductService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _product = database.GetCollection<Product>(settings.ProductStoreCollectionName);
        }

        #region List
        public List<Product> Get() =>
            _product.Find<Product>(x => true).ToList();
        public async Task<List<Product>> GetAsync() =>
             await _product.Find<Product>(x => true).ToListAsync();

        public Product Get(string id) =>
            _product.Find<Product>(x => x.Id == id).FirstOrDefault();
        public async Task<Product> GetAsync(string id) =>
           await _product.Find<Product>(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<List<Product>> GetByTagsAsync(string tag)
        {
            List<Product> t = new List<Product>();
            if (string.IsNullOrEmpty(tag))
                t = await _product.Find<Product>(x => x.Tags != null).ToListAsync();
            else
                t = await _product.Find<Product>(x => x.Tags.Contains(tag)).ToListAsync();
            return t;
        }
        public List<Product> GetByKeyword(string keyword) =>
         _product.Find<Product>(x => x.Keyword == keyword).ToList();

        public async Task<List<Product>> GetByKeywordAsync(string keyword) =>
         await _product.Find<Product>(x => x.Keyword.ToLower() == keyword.ToLower()).ToListAsync();

        public async Task<List<Product>> GetLikeKeywordAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {

                return await _product.Find<Product>(x => true).ToListAsync();
            }
            else
            {
                return await _product.Find<Product>(x => x.Keyword.ToLower().StartsWith(keyword.ToLower())).ToListAsync();
            }
        }
        #endregion



        public Product Create(Product x)
        {
            var now = DateTime.Now;
            x.Created = now;
            x.Modified = now;
            _product.InsertOne(x);
            return x;
        }

        public void Update(string id, Product xIn)
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





