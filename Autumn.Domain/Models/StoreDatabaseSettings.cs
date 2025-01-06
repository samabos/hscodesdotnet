using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.Domain.Models
{

    public class StoreDatabaseSettings : IStoreDatabaseSettings
    {
        public string HSCodeStoreCollectionName { get; set; }
        public string ProductStoreCollectionName { get; set; }
        public string Product2StoreCollectionName { get; set; }
        public string KeywordStoreCollectionName { get; set; }
        public string SearchLogStoreCollectionName { get; set; }
        public string DocumentStoreCollectionName { get; set; }
        public string CurrencyStoreCollectionName { get; set; }
        public string IdentityStoreCollectionName { get; set; }
        public string CustomsTariffStoreCollectionName { get; set; }
        public string HSCodeToDocumentStoreCollectionName { get; set; }
        public string RequirementStoreCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IStoreDatabaseSettings
    {
        string HSCodeStoreCollectionName { get; set; }
        string ProductStoreCollectionName { get; set; }
        string Product2StoreCollectionName { get; set; }
        string KeywordStoreCollectionName { get; set; }
        string SearchLogStoreCollectionName { get; set; }
        string DocumentStoreCollectionName { get; set; }
        string CurrencyStoreCollectionName { get; set; }
        string IdentityStoreCollectionName { get; set; }
        string CustomsTariffStoreCollectionName { get; set; }
        string HSCodeToDocumentStoreCollectionName { get; set; }
        string RequirementStoreCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
