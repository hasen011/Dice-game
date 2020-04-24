using System;
using System.Collections.Generic;
using System.Linq;

namespace Probability_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var pattern = new[] { 1, 2, 3, 4, 5, 6 };
            var currentDice = new[] { 4, 2, 2, 4, 2, 4 };


            foreach (var x in MissingDice(currentDice, pattern))
            {
                Console.WriteLine(x);
            }
        }

        /// <summary>
        /// This method find a difference between two dice sets, i.e. finds missing dice from a current roll to complete a desired pattern.
        /// </summary>
        /// <param name="dice1"></param>
        /// <param name="dice2"></param>
        static List<int> MissingDice(int[] currentDice, int[] pattern)
        {
            Array.Sort(currentDice);
            Array.Sort(pattern);

            var result = new List<int>(6);

            for (var i = 0; i < currentDice.Length; i++)
            {
                if (currentDice[i] != pattern[i])
                {
                    result.Add(pattern[i]);
                }
            }

            return result;
        }
    }
}
