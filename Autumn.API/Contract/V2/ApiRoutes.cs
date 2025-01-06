using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.API.Contract.V2
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v2";
        //public const string Base = Root + "/" + Version;
        public const string Base =  Version;
        public static class Search
        {
            public const string Get = Base + "/search";
        }
    }
}
