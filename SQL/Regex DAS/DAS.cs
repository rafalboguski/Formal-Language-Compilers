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
        private List<State> States;
        private List<string> Keys;
        private string temporaryFoundElement;
        private List<string> FoundElements;

        public DAS(List<string> keys = null, List<State> states = null)
        {
            States = states == null ? new List<State>()
            {
                new State(0,  new Dictionary<string, int>(){{ "[A-Z]", 1 } }),
                new State(1,  new Dictionary<string, int>(){{ "[A-Z]", 2 } }),
                new State(2,  new Dictionary<string, int>(){{ "[0-9]", 4 }, { " ", 3}}),
                new State(3,  new Dictionary<string, int>(){{ "[0-9]", 4 } }),
                new State(4,  new Dictionary<string, int>(){{ "[A-Z]", 8 }, { "[0-9]", 5}}),
                new State(5,  new Dictionary<string, int>(){{ "[0-9]", 6 } }),
                new State(6,  new Dictionary<string, int>(){{ "[A-Z]", 11}, { "[0-9]", 7 } }),
                new State(7,  new Dictionary<string, int>(){{ "[A-Z]", 12}, { "[0-9]", 12 } }),
                new State(8,  new Dictionary<string, int>(){{ "[A-Z]", 9 }, { "[0-9]", 9 } }),
                new State(9,  new Dictionary<string, int>(){{ "[0-9]", 10} }),
                new State(10, new Dictionary<string, int>(){{ "[0-9]", 12} }),
                new State(11, new Dictionary<string, int>(){{ "[A-Z]", 12} }),
                new State(12, new Dictionary<string, int>(){}),
            } : states;

            Keys = keys == null ? new List<string>() { "[A-Z]", "[0-9]", " " } : keys;

            CurrentState = States.First();
            FoundElements = new List<string>();
        }

        public List<string> Start(string input)
        {
            foreach (var key in input)
            {
                ChangeState(key.ToString());
            }
            return FoundElements;
        }

        private void ChangeState(string inKey)
        {
            if (CurrentState.Number == States.First().Number)
                temporaryFoundElement = "";

            foreach (var key in Keys)
            {
                if (Regex.IsMatch(inKey, $@"^{key}") && CurrentState.Moves.ContainsKey(key))
                {
                    CurrentState = States.ElementAt(CurrentState.Moves[key]);
                    temporaryFoundElement += inKey;
                    if (CurrentState.Number == States.Last().Number)
                    {
                        FoundElements.Add(temporaryFoundElement);
                        CurrentState = States.First();
                    }
                    break;
                }
            }
        }

    }

    class State
    {
        public int Number;
        public Dictionary<string, int> Moves;

        public State(int stateNumber, Dictionary<string, int> movesTable)
        {
            Number = stateNumber;
            Moves = movesTable;
        }
    }
}
