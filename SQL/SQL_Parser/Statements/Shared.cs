using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQL_Parser.Statements
{
    public class Word
    {
        public List<Word> WordsSequence { get; set; } = new List<Word>();
        public List<Word> WordsAlternative { get; set; } = new List<Word>();

        // pattern
        protected string _regex;
        // combined from all inner wordds
        public string Regeex
        {
            get
            {
                if (Words.Count() == 0)
                    return _regex;

                var reg = @"";
                foreach (var word in Words)
                {
                    reg += "(" + word.Regeex + ")";
                }

                return reg;
            }
            set { }
        }
                 
        public string Text { get; }


        public bool IsMatch(string input)
        {
            return new Regex(this.Regeex).IsMatch(input);
        }

        public Match Match(string input)
        {
            return new Regex(this.Regeex).Match(input);
        }
    }



    class Word_semicolon : Word
    {
        public Word_semicolon()
        {
            _regex = ";";
        }
    }
}
