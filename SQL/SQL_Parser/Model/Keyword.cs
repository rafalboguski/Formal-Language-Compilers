using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Parser.Model
{
    class Keyword
    {
        public string Name { get; set; }
        public string Regex { get; set; }

        public Keyword(string name, string regex)
        {
            this.Name = name;
            this.Regex = regex;
        }
    }
}
