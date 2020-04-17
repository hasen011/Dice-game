using Dice_game.PlayerDomain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dice_game.Infrastructure
{
    public class CombinationList
    {
        private readonly ILookup<int[], Combination> Combinations;

        public CombinationList()
        {
            // Load list of all possible combinations (dice patterns)
            var combinations = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Infrastructure\\Combinations.txt"));

            // Maps dice (combination) pattern to a combination object
            Combinations = combinations.ToLookup(
                    x => {
                        var dicePattern = x.Split(" ")[0].Split(",").Select(int.Parse).ToArray();

                        return dicePattern;
                    },
                    x => {
                        var dicePattern = x.Split(" ")[0].Split(",").Select(int.Parse).ToArray();
                        Enum.TryParse(x.Split(" ")[1], true, out CombinationType combinationType);

                        return new Combination(combinationType, dicePattern);
                    },
                    new MyEqualityComparer()
                );
        }

        // TODO: Doesn't really work, need to check subsets - figure out a way
        public List<Combination> LookupCombination(int[] dice)
        {
            // Sort the array to match with combination patterns
            var sortedDice = dice.OrderBy(x => x).ToArray();
            var matchingCombinations = new List<Combination>();

            for (var i = 3; i <= 6; i++)
            {
                // Try to find any matching combination patterns
                matchingCombinations.AddRange(Combinations[sortedDice.Take(i).ToArray()]);
            }

            return matchingCombinations;
        }
    }

    // Custom comparer to allow arrays as keys in Dictionaries/Lookups
    public class MyEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int[] obj)
        {
            // Hash code obtained here: https://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-overriding-gethashcode
            int result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked // Overflow is fine, just wrap
                {
                    result = result * 23 + obj[i];
                }
            }
            return result;
        }
    }
}
