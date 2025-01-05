using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V1.Requests
{
    public class UserRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
