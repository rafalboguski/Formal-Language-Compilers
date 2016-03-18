using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SQL_Parser
{
    class Test
    {
        public static void Main(string[] argc)
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
            var parser = new ParserSql();

            parser.ValidateSQL();
           

        }
    }
}
