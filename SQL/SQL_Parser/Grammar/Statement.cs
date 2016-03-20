﻿using SQL_Parser.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQL_Parser.Grammar
{

    public class GrammarError
    {

        public GrammarError(Token token, string expected)
        {
            this.Token = token;
            this.Expected = expected;
        }

        public Token Token { get; set; }
        public string Expected { get; set; }

        public override string ToString()
        {
            return "Expected token: <" + Expected + "> Found " + Token;
        }

    }

    class Word
    {
        public static List<GrammarError> GrammarErrors { get; set; } = new List<GrammarError>();
        public List<GrammarError> wordErrors { get; set; } = new List<GrammarError>();

        public string Name { get; set; }

        // pierwsza lista to alternatywy kolejna to sekwencje
        public List<List<Word>> Words { get; set; }

        public bool loop { get; set; }
        public bool loopWithoutAnyWords { get; set; }

        public bool IsMatch(ref List<Token> tokens, Word parentWord = null)
        {
            #region Liść

            if (Words == null)
            {
                if (tokens.Any() == false)
                {
                    return false;
                }

                var token = tokens.First();

                // change to names digits and so on
                if (token.userMade == true && Name == "userMade")
                {
                    Debug.WriteLine(token.Index + "\tin: <" + token.Name + ">\t expected: <" + Name + ">\t true");

                    tokens.RemoveAt(0);
                    return true;
                }

                if (token.Name == Name)
                {
                    Debug.WriteLine(token.Index + "\tin: <" + token.Name + ">\t expected: <" + Name + ">\t true");

                    tokens.RemoveAt(0);
                    return true;
                }
                else
                {   //if(Name != null)
                    //GrammarErrors.Add(new GrammarError(token, Name));
                    return false;
                }
            }
            #endregion

            #region Rekurencja
            var alternatives = Words;
            foreach (var sequence in alternatives)
            {
                var sequenceMatch = false;

                var tokensCopy = new List<Token>();
                tokensCopy.AddRange(tokens);

                #region Pętla
                if (loop)
                {
                    var loops = 0;
                    var continueLoop = true;
                    int done = 0;
                    var tokensInLoop = new List<Token>();

                    while (continueLoop)
                    {
                        tokensInLoop.Clear();
                        tokensInLoop.AddRange(tokensCopy);

                        var loopSexentce = new List<Word>(sequence);
                        for (int i = 0; i < loops; i++)
                            loopSexentce.AddRange(sequence);


                        done = 0;
                        foreach (var word in loopSexentce)
                        {
                            if (word.IsMatch(ref tokensInLoop, this) == false)
                            {    // tylko jak cala sekwencja jest poprawna powinien zwrocic tokeny
                                if (loops >= 1)
                                {
                                    sequenceMatch = true;
                                    continueLoop = false;
                                    continue;
                                }
                                else
                                {
                                    if (loopWithoutAnyWords && loops == 0)
                                    {
                                        sequenceMatch = true;
                                        continueLoop = false;
                                        continue;
                                    }

                                    if (tokensInLoop.Any() && word.Name != null)
                                        GrammarErrors.Add(new GrammarError(tokensInLoop.First(), word.Name));
                                    sequenceMatch = false;
                                    continueLoop = false;
                                    break;
                                }

                            }
                            done++;
                        }
                        loops++;
                    }
                    if (sequenceMatch)
                    {
                        tokens = tokensCopy;
                        if ((done % sequence.Count()) == 0)
                            tokens = tokensInLoop;
                        else
                        {
                            var take = ((int)(((float)done) / ((float)sequence.Count()))) * sequence.Count();
                            tokens.RemoveRange(0, take);
                        }
                        GrammarErrors.Clear();

                        return true;
                    }
                }
                #endregion
                else
                    foreach (var word in sequence)
                    {
                        if (word.IsMatch(ref tokensCopy, this) == false)
                        {
                            if (tokensCopy.Any() && word.Name != null)
                                GrammarErrors.Add(new GrammarError(tokensCopy.First(), word.Name));
                            sequenceMatch = false;
                            break;
                        }
                        else
                        {
                            sequenceMatch = true;
                        }
                    }

                if (sequenceMatch)
                {
                    tokens = tokensCopy;
                    return true;
                }
            }
            #endregion

            return false;
        }

    }

    class Word_STATEMENTS : Word
    {

        public Word_STATEMENTS()
        {
            Words = new List<List<Word>>();
            Words.Add(new List<Word>() { new Word_STATEMENT() });

            loop = true;

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
                new Word_SELECT_Columns(),

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

    class Word_SELECT_Columns : Word
    {
        public Word_SELECT_Columns()
        {
            Words = new List<List<Word>>();
            //Words.Add(new List<Word>()
            //{
            //    new Word_userMade(),
            //    new Word_space(),

            //});
            Words.Add(new List<Word>()
            {
                new Word_SELECT_Columns_loop(),
                new Word_userMade(),
                new Word_space(),

            });
        }

        private class Word_SELECT_Columns_loop : Word
        {
            public Word_SELECT_Columns_loop()
            {
                Words = new List<List<Word>>();
                Words.Add(new List<Word>()
                {
                    new Word_userMade(),
                    new Word_comma(),
                    // spaceornot
                   
                });

                loop = true;
                loopWithoutAnyWords = true;
            }

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


    class Word_space : Word
    {
        public Word_space()
        {
            Name = "space";
        }
    }
    class Word_semicolon : Word
    {
        public Word_semicolon()
        {
            Name = "semicolon";
        }
    }
    class Word_comma : Word
    {
        public Word_comma()
        {
            Name = "comma";
        }
    }

    #endregion
}