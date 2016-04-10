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
            var input = "kupiłem tablice AA 34334 i BB32434";
            var replaceString = "<plate number>";
            var licensePlateRegex = new DAS();

            WriteLine($"Replace license plate number with string \"{replaceString}\"\n");
            WriteLine($"INPUT:\n\n{input}\n\n");




            WriteLine($"OUTPUT:\n\n");

            ReadLine();
        }
    }
}
