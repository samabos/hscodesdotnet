using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V1.Responses
{
    public class NoteResponse
    {
        
        public IEnumerable<string> Error { get; set; }

        public bool Success { get; set; }

        public List<HSCodeObject> Records { get; set; }
        public List<DocumentObject> Documents { get; set; }
        public List<CustomsTariffObject> Tariff { get; set; }
        public List<HscodeToDocumentObject> RecordsToDocuments { get; set; }
    }
    

}
