using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class Validation
    {
        internal bool ValidateInput(string input)
        {

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("You must enter something! Try again!");
                Console.WriteLine("");
                return false;
            }
            else
                return true;
        }
        internal bool ValidateInput(int choice, CodingSession session)
        {
            if (session == null)
            {
                Console.WriteLine("Invalid ID entered! Try again!");
                Console.WriteLine("");
                return false;
            }
            else
                return true;
        }
    }
}
