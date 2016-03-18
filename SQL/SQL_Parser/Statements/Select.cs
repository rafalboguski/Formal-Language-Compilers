using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQL_Parser.Statements
{

    class Word_SELECT : Word
    {
        public Word_SELECT()
        {
            Words.Add(new Word_select());
            Words.Add(new Word_semicolon());

        }


        class Word_select : Word
        {
            public Word_select()
            {
                _regex = "(select|SELECT)";

            }
        }

    }

}
