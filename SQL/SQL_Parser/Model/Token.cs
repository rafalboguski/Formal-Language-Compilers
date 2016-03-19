using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQL_Parser.Model
{
    public class Token
    {
        public string Name { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }

        public bool userMade { get; set; }

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


        public override string ToString()
        {
            return "Token { Name: " + Name + ", Index: " + Index + ", Length: " + Length + " }";
        }
    }
}
