using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Services
{

    public class RequirementService
    {
        private readonly IMongoCollection<Requirement> _requiremnt;

        public RequirementService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _requiremnt = database.GetCollection<Requirement>(settings.RequirementStoreCollectionName);
        }

        #region List
        public List<Requirement> Get() =>
            _requiremnt.Find<Requirement>(x => true).ToList();
        public async Task<List<Requirement>> GetAsync() =>
             await _requiremnt.Find<Requirement>(x => true).ToListAsync();

        public Requirement Get(string id) =>
            _requiremnt.Find<Requirement>(x => x.Id == id).FirstOrDefault();
        public async Task<Requirement> GetAsync(string id) =>
           await _requiremnt.Find<Requirement>(x => x.Id == id).FirstOrDefaultAsync();

        
        public List<Requirement> GetByHSCode(string keyword) =>
         _requiremnt.Find<Requirement>(x => x.HSCode == keyword).ToList();

        public async Task<List<Requirement>> GetByHSCodeAsync(string keyword) =>
         await _requiremnt.Find<Requirement>(x => x.HSCode == keyword).ToListAsync();

        public async Task<List<Requirement>> GetLikeKeywordAsync(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {

                return await _requiremnt.Find<Requirement>(x => true).ToListAsync();
            }
            else
            {
                return await _requiremnt.Find<Requirement>(x => x.HSCode.ToLower().StartsWith(keyword.ToLower())).ToListAsync();
            }
        }
        #endregion

        public Requirement Create(Requirement x)
        {
            _requiremnt.InsertOne(x);
            return x;
        }

        public void Update(string id, Requirement xIn) =>
            _requiremnt.ReplaceOne(x => x.Id == id, xIn);

        public void Remove(Requirement xIn) =>
            _requiremnt.DeleteOne(x => x.Id == xIn.Id);

        public void Remove(string id) =>
            _requiremnt.DeleteOne(x => x.Id == id);
    }
}
