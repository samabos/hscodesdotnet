using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Autumn.UI.Contract.V1.Responses
{
    public class HscodeToDocumentObject
    {
        public string Id { get; set; }
        public string Country { get; set; }
        public string Agency { get; set; }
        public string Hscode { get; set; }
        public string HscodeLocal { get; set; }
        public string Description { get; set; }
        public string ImpGeneral { get; set; }
        public string ImpFinishedProductsInRetailPack { get; set; }
        public string ImpBulkConsignments { get; set; }
        public string ImpChemicalsOrRawMaterials { get; set; }
        public string ImpSupermktOrRestaurant { get; set; }
        public string ExpGeneral { get; set; }
    }
}
