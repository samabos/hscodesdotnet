using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.UI.Contract.V1.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Error { get; set; }
    }
}
