using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace Regex_DAS
{
    class Program
    {
        static void Main(string[] args)
        {
            var licensePlateRegex = new DAS();
            var replaceString = "<TABLICA REJ>";
            var input = "Pan Jan ma tablice rejestracyjne o numerach YX 67893, YX 67893 oraz XY 1234A,\n" +
                        "XY 123AC, XY 1A234, XY 1AC23";

            WriteLine($"INPUT---------------------------------------\n\n{input}\n\n");

            var foundElements = licensePlateRegex.Start(input);

            WriteLine($"OUTPUT--------------------------------------\n");
            var output = input;
            foreach (var x in foundElements)
                output = output.Replace(x, replaceString);

            WriteLine(output);

            ReadLine();
        }
    }
}
