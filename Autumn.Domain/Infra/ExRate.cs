using Autumn.Domain.Models;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;

namespace Autumn.Domain.Infra
{
    public class ExRate:IExRate
    {
        public IList<CBNEXRate> Load() {
            var webRequest = WebRequest.Create(@"https://www.cbn.gov.ng/Functions/export.asp?tablename=exchange");

            using (var response = webRequest.GetResponse())
            using (var content = response.GetResponseStream())
            using (var reader = new StreamReader(content))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = csv.GetRecords<CBNEXRate>();
                //var rec = records.FirstOrDefault();
                return records.ToList();
            }
        }
    }
}
