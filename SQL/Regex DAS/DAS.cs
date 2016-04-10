using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Text.RegularExpressions;

namespace Regex_DAS
{
    class DAS
    {
        private State CurrentState;
        private List<State> StatesTable;
        private List<string> FoundElements;

        public DAS()
        {
            StatesTable = new List<State>()
            {
                new State(0,  new Dictionary<string, int>(){{ "[A-Z]", 1}}),
                new State(1,  new Dictionary<string, int>(){{ "[A-Z]", 2}}),
                new State(2,  new Dictionary<string, int>(){{ " ", 3}, { "[0-9]", 4}}),
                new State(3,  new Dictionary<string, int>(){{ "[0-9]", 4}}),
                new State(4,  new Dictionary<string, int>(){{ "[A-Z]", 8}, { "[0-9]", 5}}),
                new State(5,  new Dictionary<string, int>(){{ "[0-9]", 6} }),
                new State(6,  new Dictionary<string, int>(){{ "[A-Z]", 11}, { "[0-9]", 7 } }),
                new State(7,  new Dictionary<string, int>(){{ "[A-Z]", 12}, { "[0-9]", 12 } }),
                new State(8,  new Dictionary<string, int>(){{ "[A-Z]", 9 }, { "[0-9]", 9 } }),
                new State(9,  new Dictionary<string, int>(){{ "[0-9]", 10} }),
                new State(10, new Dictionary<string, int>(){{ "[0-9]", 12} }),
                new State(11, new Dictionary<string, int>(){{ "[A-Z]", 12} }),
                new State(12, new Dictionary<string, int>(){}),


            };

            CurrentState = StatesTable.First();
            FoundElements = new List<string>();
        }

        private string temporaryFoundElement = "";

        public List<string> Start(string input)
        {
            foreach (var key in input)
            {
                ChangeState(key);
            }
            return FoundElements;
        }

        private void ChangeState(char inKey)
        {
            var keys = new List<string>()
            {
                "[A-Z]",
                "[0-9]",
                " "
            };

            if (CurrentState.Number == StatesTable.First().Number)
                temporaryFoundElement = "";

            foreach (var key in keys)
            {
                if (Regex.IsMatch(inKey.ToString(), $@"^{key}") && CurrentState.MovesTable.ContainsKey(key.ToString()))
                {
                    CurrentState = StatesTable.ElementAt(CurrentState.MovesTable[key]);
                    temporaryFoundElement += inKey;
                    if (CurrentState.Number == StatesTable.Last().Number)
                    {
                        FoundElements.Add(temporaryFoundElement);
                        CurrentState = StatesTable.First();
                    }
                    break;
                }
            }
        }

    }

    class State
    {
        public int Number;
        public Dictionary<string, int> MovesTable;

        public State(int number, Dictionary<string, int> moves)
        {
            Number = number;
            MovesTable = moves;
        }

        public override string ToString()
        {
            return $"State {Number}";
        }
    }
}
