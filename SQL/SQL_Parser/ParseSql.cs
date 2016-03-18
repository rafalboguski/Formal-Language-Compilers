using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using SQL_Parser.Statements;

namespace SQL_Parser
{

    class ParserSql
    {

        private List<Word> _grammar;


        public ParserSql()
        {
            _grammar = new List<Word>();

           // _grammar.Add(new Word_SELECT());


            var SEL = new Word_SELECT();

            var r = SEL.Regeex;

            var boo = SEL.IsMatch("select;");
            var boo2 = SEL.IsMatch("select ;");
            var boo3 = SEL.IsMatch("SELECT;");

              

            var sdfsdf = 4;
        }

    }

    

}
