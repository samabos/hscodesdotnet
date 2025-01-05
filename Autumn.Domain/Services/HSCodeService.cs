using Autumn.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Services
{

    public class HSCodeService
    {
        private readonly IMongoCollection<HSCode> _hscode;

        public HSCodeService(IStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _hscode = database.GetCollection<HSCode>(settings.HSCodeStoreCollectionName);
        }

        public List<HSCode> Get() =>
            _hscode.Find<HSCode>(x => true).ToList();
        public async Task<List<HSCode>> GetAsync() =>
             await _hscode.Find<HSCode>(x => true).ToListAsync();

        public HSCode Get(string id) =>
            _hscode.Find<HSCode>(x => x.PId == id).FirstOrDefault();

        public async Task<HSCode> GetAsync(string id) =>
          await  _hscode.Find<HSCode>(x => x.PId == id).FirstOrDefaultAsync();


        public async Task<List<HSCode>> GetWithOptionsAsync(string id, string pid, string level)
        {
            IEnumerable<HSCode> resp = new List<HSCode>();
            if (string.IsNullOrEmpty(id) && string.IsNullOrEmpty(pid) && string.IsNullOrEmpty(level))
            {
                resp = await _hscode.Find<HSCode>(x => x.Level == 1).ToListAsync();
            }
            else if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(level))
            {
                resp = await _hscode.Find<HSCode>(x => x.Level == long.Parse(level) && x.ParentId == pid).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(pid) && string.IsNullOrEmpty(level))
            {

                resp = await _hscode.Find<HSCode>(x => x.Id == id).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(id) && string.IsNullOrEmpty(pid) && !string.IsNullOrEmpty(level))
            {
                var f = await _hscode.Find<HSCode>(x => x.Id == id && x.Level == long.Parse(level)).FirstOrDefaultAsync();
                resp = await _hscode.Find<HSCode>(x => x.ParentId == f.ParentId && x.Level == long.Parse(level)).ToListAsync();
            }
            else if (string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(pid) && string.IsNullOrEmpty(level))
            {
                resp = await _hscode.Find<HSCode>(x => x.ParentId == pid).ToListAsync();
            }
            return resp.OrderBy(x => x.Order).ToList();
        }

        public async Task<List<HSCode>> GetWithHSCodeOptionsAsync(string code, string pcode, string level)
        {
            IEnumerable<HSCode> resp = new List<HSCode>();
            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(pcode) && string.IsNullOrEmpty(level))
            {
                resp = await _hscode.Find<HSCode>(x => x.Level == 1).ToListAsync();
            }
            else if (string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(pcode) && !string.IsNullOrEmpty(level))
            {
                resp = await _hscode.Find<HSCode>(x => x.Level == long.Parse(level) && x.ParentCode == pcode).ToListAsync();
            }
            else if (!string.IsNullOrEmpty(code) && string.IsNullOrEmpty(pcode) && string.IsNullOrEmpty(level))
            {

                resp = await _hscode.Find<HSCode>(x => x.Code == code).ToListAsync();
            }
            else if (string.IsNullOrEmpty(code) && !string.IsNullOrEmpty(pcode) && string.IsNullOrEmpty(level))
            {
                resp = await _hscode.Find<HSCode>(x => x.ParentCode == pcode).ToListAsync();
                if (!resp.Any())
                    resp = await _hscode.Find<HSCode>(x => x.Code.StartsWith(pcode)).ToListAsync();
            }
            return resp.OrderBy(x => x.Order).ToList();
        }
        

        public HSCode Create(HSCode x)
        {
            _hscode.InsertOne(x);
            return x;
        }

        public void Update(string id, HSCode xIn) =>
            _hscode.ReplaceOne(x => x.PId == id, xIn);

        public void Remove(HSCode xIn) =>
            _hscode.DeleteOne(x => x.PId == xIn.PId);

        public void Remove(string id) =>
            _hscode.DeleteOne(x => x.PId == id);
    }
}
