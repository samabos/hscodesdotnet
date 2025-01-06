using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.UI.Contract.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;
        public static class Search
        {
            public const string Get = Base + "/search";
        }
        public static class Note
        {
            public const string Get = Base + "/note/{hscode}";
        }
        public static class Duty
        {
            public const string Get = Base + "/duty";
        }
        public static class CodeList
        {
            public const string Currency = Base + "/codelist/currency";
            public const string Tags = Base + "/codelist/tags/{query?}";
            public const string Products = Base + "/codelist/products/{query?}";
        }
        public static class Identity
        {
            public const string Login = Base + "/identity/login";
            public const string Register = Base + "/identity/register";
        }

        public static class Keyword
        {
            public const string Get = Base + "/keyword/get";
        }
    }
}
