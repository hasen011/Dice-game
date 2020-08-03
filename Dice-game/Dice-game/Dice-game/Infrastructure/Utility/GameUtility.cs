using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dice_game.Infrastructure.Utility
{
    public static class GameUtility
    {
        public static char[] CreateGenericPatternFromDice(int[] dice)
        {
            // Dice combinations are converted into a generic patterns regardless of their actual value, i.e.
            // [4,5,5,5,1,3] => [a,a,a,b,c,d]
            // This makes it easy to find out what is a probability to complete each possible combination
            var pattern = new List<char>(dice.Length);
            var patternCharacters = new char[] { 'a', 'b', 'c', 'd', 'e', 'f' };

            // Sort the dice by the frequencies of each value and substitute with an alphabet letter
            // This could probably be done in a more elegant way but I like the simplicity and the idea
            // of a 'math' substitution :-)
            var diceCounts = dice.GroupBy(x => x).OrderByDescending(x => x.Count()).ToArray();

            for (var i = 0; i < diceCounts.Count(); i++)
            {
                pattern.AddRange(Enumerable.Repeat(patternCharacters[i], diceCounts[i].Count()));
            }

            return pattern.ToArray();
        }

        public static HashSet<char[]> GetPatterns()
        {
            // Load list of all possible generic patterns
            var patterns = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Database\Patterns.txt"));

            var set = patterns.Select(x => x.ToCharArray()).ToHashSet(new ArrayEqualityComparerChar());
            return set;
        }

        /// <summary>
        /// Reads the list of pattern probabilities and returns a dictionary of format
        /// [pattern+diceLeftRoll, probabilities]
        /// </summary>
        /// <returns></returns>
        public static PatternProbability[] GetPatternProbabilities()
        {
            // Load list of all pattern probabilities. This list has each possible pattern with a probability of completing it within
            // a fixed (100) number of rolls
            var file = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Database\PatternProbabilities.txt"));

            var patternProbabilities = file.Select(p =>
            {
                var split = p.Split(" ");
                var probabilities = new List<decimal>(100);
                for (int i = 2; i < split.Length; i++)
                {
                    probabilities.Add(decimal.Parse(split[i]));
                }

                return new PatternProbability(split[0].ToCharArray(), int.Parse(split[1]), probabilities.ToArray());
            }).ToArray();

            return patternProbabilities;
        }

        public static string GetPatternProbabilityId(char[] pattern, int rollingDiceCount)
        {
            return new string(pattern) + rollingDiceCount.ToString();
        }

        public static Dictionary<string, decimal[]> CreatePatternProbabilitiesDictionary()
        {
            return GetPatternProbabilities().ToDictionary(p => p.Id, p => p.PatternProbabilities);
        }



    }




}
