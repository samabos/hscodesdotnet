using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Autumn.UI.Options
{
    public static class StaticString
    {
        public static string Heading => "1";
        public static string Chapter => "1";
        public static string Section => "1";
        public static string Code => "1";

        public static string[] Structure => new string [] {"Section","Chapter","Heading","Code"};
    }
}
