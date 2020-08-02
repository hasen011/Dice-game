using Dice_game.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dice_game.Infrastructure
{
    public class CombinationList
    {
        private readonly ILookup<int[], Combination> CombinationsLookup;
        private readonly Dictionary<int[], Combination[]> RollListWithMatchingCombinations;
        public readonly Combination[] Combinations;

        public CombinationList()
        {
            // Load list of all possible combinations (dice patterns)
            var combinations = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Database\Combinations.txt"));

            Combinations = combinations.Select(c =>
            {
                var temp = c.Split(" ");
                var dicePattern = temp[0].ToCharArray().Select(x => x - '0').ToArray();
                Enum.TryParse(temp[1], true, out CombinationType combinationType);

                return new Combination(combinationType, dicePattern);
            }).ToArray();

            // Maps dice (combination) pattern to a combination object
            CombinationsLookup = Combinations.ToLookup(x => x.Dice, x => x, new ArrayEqualityComparer());

            RollListWithMatchingCombinations = ReadFullRollListWithAllMatchingCombinations();
        }

        public Combination[] LookupMatchingCombinations(int[] dice)
        {
            // Find all possible combinations from the given dice (roll)
            return RollListWithMatchingCombinations[dice];
        }

        public Dictionary<int[], Combination[]> ReadFullRollListWithAllMatchingCombinations()
        {
            var rolls = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Database\FullRollListWithCombinations.txt"));

            var result = new Dictionary<int[], Combination[]>(46656, new ArrayEqualityComparer());

            foreach (var c in rolls)
            {
                var split = c.Split(" ");
                var combinations = new List<Combination>(10);

                for (var i = 1; i < split.Length; i++)
                {
                    combinations.AddRange(CombinationsLookup[split[i].ToCharArray().Select(x => x - '0').ToArray()]);
                }

                result.Add(split[0].ToCharArray().Select(x => x - '0').ToArray(), combinations.ToArray());
            }

            return result;
        }



    }

    
}
