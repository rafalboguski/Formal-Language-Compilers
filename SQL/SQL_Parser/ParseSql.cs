using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace SQL_Parser
{

    class ParserSql
    {


        private List<Keyword> _keywords = new List<Keyword>()
        {
            new Keyword("select", "(SELECT|select)"),
            new Keyword("from", "(FROM|from)"),
            new Keyword("where", "(WHERE|where)"),
            new Keyword("space", @"(\s+)"),
            new Keyword("semicolon", "(;)")
        };


        public ParserSql()
        {
            var INPUT = "select NAME from   NAME;";
            Console.WriteLine(INPUT);
            var keywordMatches = new List<Token>();

            // tokenize keywords
            foreach (var keyword in _keywords)
            {
                var reg = new Regex(keyword.Regex);
                var matches = reg.Matches(INPUT);

                foreach (Match match in matches)
                {
                    keywordMatches.Add(new Token(keyword.Name, match));
                }

                var t = 234;


            }
            keywordMatches = keywordMatches.OrderBy(x => x.Index).ToList();
            Console.WriteLine("keywords");
            foreach (var token in keywordMatches)
            {
                Console.Write("<" + token.Name + "> ");
            }

            // tokenize rest
            var tokens = new List<Token>(keywordMatches);

            for (int i = 0; i < keywordMatches.Count() - 1; i++)
            {
                var array = keywordMatches.ToArray();
                var token = array[i];
                var nextToken = array[i + 1];



                if (nextToken.Index != token.Index + token.Length)
                {
                    var index = token.Index + token.Length;
                    var length = nextToken.Index - (token.Index + token.Length);
                    var text = INPUT.Substring(index, length);
                    tokens.Add(new Token(text, index, length));
                }

            }

            tokens = tokens.OrderBy(x => x.Index).ToList();




            Console.WriteLine("\nall");

            foreach (var token in tokens)
            {
                Console.Write("<" + token.Name + "> ");
            }

            // validate



            var OUTPUT = "";
            Console.Read();
        }



    }

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

    class Token
    {

        public string Name { get; set; }

        public int Index { get; set; }
        public int Length { get; set; }



        public Token(string name, Match match)
        {
            Name = name;
            Index = match.Index;
            Length = match.Length;
        }
        public Token(string name, int index, int length)
        {
            Name = name;
            Index = index;
            Length = length;
        }

    }

}
