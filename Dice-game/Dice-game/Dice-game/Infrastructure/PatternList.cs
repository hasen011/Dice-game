using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace Dice_game.Infrastructure
{
    public class PatternList
    {
        private Random Random { get; set; }
        private readonly Dictionary<char[], List<decimal>> PatternProbabilities;     

        public PatternList()
        {
            Random = new Random();
            // Load list of all possible generic patterns
            var patterns = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Infrastructure\Database\Patterns.txt"));

            /*Combinations = combinations.Select(c =>
            {
                var temp = c.Split(" ");
                var dicePattern = temp[0].ToCharArray().Select(x => x - '0').ToArray();
                Enum.TryParse(temp[1], true, out CombinationType combinationType);

                return new Combination(combinationType, dicePattern);
            }).ToArray();

            // Maps dice (combination) pattern to a combination object
            CombinationsLookup = Combinations.ToLookup(x => x.Dice, x => x, new ArrayEqualityComparer());

            RollListWithMatchingCombinations = ReadFullRollListWithAllMatchingCombinations();*/
        }

        public void CreatePatternProbabilities(int numberOfRolls)
        {
            var patterns = new List<char[]>
            {
                new char[] {'a'},
                new char[] {'a', 'a'},
                new char[] {'a', 'a', 'a'},
                new char[] {'a', 'a', 'a', 'a'},
                new char[] {'a', 'a', 'a', 'a', 'a'},
                new char[] {'a', 'a', 'a', 'a', 'a', 'a'},
                new char[] {'a', 'b'},
                new char[] {'a', 'b', 'c'},
                new char[] {'a', 'b', 'c', 'd'},
                new char[] {'a', 'b', 'c', 'd', 'e'},
                new char[] {'a', 'a', 'b'},
                new char[] {'a', 'a', 'a', 'b'},
                new char[] {'a', 'a', 'b', 'b'},
                new char[] {'a', 'a', 'b', 'b', 'c'},
                new char[] {'a', 'a', 'a', 'b', 'b'},
                new char[] {'a', 'a', 'a', 'b', 'b', 'b'},
                new char[] {'a', 'a', 'b', 'c'},
                new char[] {'a', 'a', 'b', 'b', 'c', 'c'},
            };

            using StreamWriter file = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Infrastructure\Database\Patterns.txt");

            for (var r = 0; r < numberOfRolls; r++)
            {


                Console.WriteLine(string.Join(",", GenerateDice(2)));
            }
        }

        private char[] MissingDiceToCompletePattern(char[] pattern, char[] rolledDice)
        {
            // It is assumed that the pattern dice are already sorted (I created them that way)
            // Also, it is assumed that rolledDice.Length >= Dice.Length
            var sortedDice = rolledDice.OrderBy(x => x).ToArray();

            var result = new List<char>(pattern.Length);

            int m = sortedDice.Length, m0 = 0, n = pattern.Length, n0 = 0;
            while (1 == 1)
            {
                // If we reached the end of combination Dice, then we can stop
                if (n0 == n)
                {
                    break;
                }

                // If we reached end of rolled dice or the rolled die value is greater than the Die,
                // then we can no longer match that Die
                if (m0 == m || sortedDice[m0] > pattern[n0])
                {
                    result.Add(pattern[n0]);
                    n0++;
                }
                // Move to the next rolled die if the value is lower than the one we need
                else if (sortedDice[m0] < pattern[n0])
                {
                    m0++;
                }
                // If we find a match then move both indexes
                else if (sortedDice[m0] == pattern[n0])
                {
                    n0++;
                    m0++;
                }
            }

            return result.ToArray();
        }

        private char[] GenerateDice(int len)
        {
            var rolledDice = new char[len];

            for (var i = 0; i < rolledDice.Length; i++)
            {
                rolledDice[i] = (char)(Random.Next(0, 6) + 'a');
            }

            return rolledDice;
        }


    }
}
