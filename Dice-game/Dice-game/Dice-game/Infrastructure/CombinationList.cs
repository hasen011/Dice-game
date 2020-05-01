using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dice_game.Infrastructure
{
    public class CombinationList
    {
        private readonly ILookup<int[], Combination> CombinationsLookup;
        public readonly Combination[] Combinations;

        public CombinationList()
        {
            // Load list of all possible combinations (dice patterns)
            var combinations = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Infrastructure\Database\Combinations.txt"));

            // Maps dice (combination) pattern to a combination object
            CombinationsLookup = combinations.ToLookup(
                    x => {
                        var dicePattern = x.Split(" ")[0].Split(",").Select(int.Parse).ToArray();

                        return dicePattern;
                    },
                    x => {
                        var dicePattern = x.Split(" ")[0].Split(",").Select(int.Parse).ToArray();
                        Enum.TryParse(x.Split(" ")[1], true, out CombinationType combinationType);

                        return new Combination(combinationType, dicePattern);
                    },
                    new ArrayEqualityComparer()
                );

            Combinations = combinations.Select(c =>
            {
                var temp = c.Split(" ");
                var dicePattern = temp[0].Split(",").Select(int.Parse).ToArray();
                Enum.TryParse(temp[1], true, out CombinationType combinationType);

                return new Combination(combinationType, dicePattern);
            }).ToArray();
        }

        public Combination[] LookupMatchingCombinations(int[] dice)
        {
            // Sort the array to match with combination patterns. Create a new array, don't want to change the underlying one.
            var sortedDice = dice.OrderBy(x => x).ToList();
            var matchingCombinations = new HashSet<Combination>();

            // Check all array slices starting at length 3 going to length 6 and find all matching
            // combinations. Use HashSet to eliminate duplicates
            for (var i = 3; i <= 6; i++)
            {
                for (var j = 0; j <= 6 - i; j++)
                {
                    // Try to find any matching combination patterns
                    var newMatchingCombinations = CombinationsLookup[sortedDice.GetRange(j, i).ToArray()];
                    foreach (var c in newMatchingCombinations)
                    {
                        matchingCombinations.Add(c);
                    }
                }             
            }

            return matchingCombinations.ToArray();
        }
    }

    
}
