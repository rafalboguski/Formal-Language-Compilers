using SQL_Parser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Parser.Grammar
{

    class Word
    {
        public string Name { get; set; }

        // pierwsza lista to alternatywy kolejna to sekwencje
        public List<List<Word>> Words { get; set; }

        public bool IsMatch(List<Token> tokens)
        {
            var match = false;

            if(Words == null)
            {
                // change to names digits and so on
                if (tokens.First().userMade == true && Name == "userMade")
                {
                    Console.WriteLine("in: " + tokens.First().Name + "\t expected: " + Name + "\t true");

                    tokens.RemoveAt(0);
                    return true;
                }

                if (tokens.First().Name == Name)
                {
                    Console.WriteLine("in: " + tokens.First().Name + "\t expected: " + Name + "\t true");

                    tokens.RemoveAt(0);
                    return true;
                }
                
                else
                {
                    return false;
                }
            }

            foreach (var alternative in Words)
            {
                var sequenceMatch = true;
                foreach (var sequence in alternative)
                {
                    if (sequence.IsMatch(tokens) == false)
                    {
                        sequenceMatch = false;
                        break;
                    }
                }

                if (sequenceMatch == true)
                {
                    return true;
                }

            }

            return match;
        }

    }

    class Word_STATEMENT : Word
    {

        public Word_STATEMENT()
        {
            Words = new List<List<Word>>();
            Words.Add(new List<Word>() { new Word_SELECT() });
            Words.Add(new List<Word>() { new Word_DELETE() });
        }

    }

    class Word_SELECT : Word
    {

        public Word_SELECT()
        {
            Words = new List<List<Word>>();

            Words.Add(new List<Word>()
            {
                new Word_select(),
                new Word_space(),
                new Word_userMade(),
                new Word_space(),
                new Word_from(),
                new Word_space(),
                new Word_userMade(),
                new Word_space(),
                new Word_where(),
                new Word_space(),
                new Word_userMade(),
                new Word_semicolon()
            });

        }



    }

    class Word_DELETE : Word
    {

        public Word_DELETE()
        {

        }

    }



    #region SHARED

    class Word_select : Word
    {
        public Word_select()
        {
            Name = "select";
        }


    }
    class Word_semicolon : Word
    {
        public Word_semicolon()
        {
            Name = "semicolon";
        }


    }
    class Word_space : Word
    {
        public Word_space()
        {
            Name = "space";
        }
    }
    class Word_from : Word
    {
        public Word_from()
        {
            Name = "from";
        }
    }
    class Word_where : Word
    {
        public Word_where()
        {
            Name = "where";
        }
    }
    class Word_userMade : Word
    {
        public Word_userMade()
        {
            Name = "userMade";
        }
    }
    #endregion
}
