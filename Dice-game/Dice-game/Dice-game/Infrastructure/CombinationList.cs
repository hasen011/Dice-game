using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
            var combinations = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Infrastructure\Database\Combinations.txt"));

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

        /// <summary>
        /// Creates a txt file with all possible rolls we can get and all possible matching combinations.
        /// </summary>
        public void CreateFullListOfAllPossibleRollsWithPossibleCombinations()
        {
            var dice = new int[] { 0, 0, 0, 0, 0, 0 }.ToList();
            var rollWithPossibleCombinations = new StringBuilder(56);
            Console.WriteLine(Directory.GetCurrentDirectory());
            using StreamWriter file = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Infrastructure\Database\FullRollListWithCombinations.txt");
            for (var a = 1; a <= 6; a++)
            {
                dice[0] = a;
                for (var b = 1; b <= 6; b++)
                {
                    dice[1] = b;
                    for (var c = 1; c <= 6; c++)
                    {
                        dice[2] = c;
                        for (var d = 1; d <= 6; d++)
                        {
                            dice[3] = d;
                            for (var e = 1; e <= 6; e++)
                            {
                                dice[4] = e;
                                for (var f = 1; f <= 6; f++)
                                {
                                    dice[5] = f;

                                    rollWithPossibleCombinations.Append(string.Join("", dice));
                                    var uniqueCombinations = new HashSet<int[]>(new ArrayEqualityComparer());

                                    // Create all possible dice subsets of length 3 or greater
                                    for (var i = 3; i <= 6; i++)
                                    {
                                        for (var j = 0; j <= 6 - i; j++)
                                        {
                                            uniqueCombinations.Add(dice.OrderBy(x => x).ToList().GetRange(j, i).ToArray());
                                        }
                                    }

                                    // Check if a subset is a combination
                                    foreach (var z in uniqueCombinations) 
                                    {
                                        if (CombinationsLookup.Contains(z))
                                        {
                                            rollWithPossibleCombinations.Append($" {string.Join("", z)}");
                                        }                                       
                                    }
                                    
                                    file.WriteLine(rollWithPossibleCombinations);
                                    rollWithPossibleCombinations.Clear();
                                }
                            }
                        }
                    }
                }
            }


        }

        public Dictionary<int[], Combination[]> ReadFullRollListWithAllMatchingCombinations()
        {
            var rolls = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), @"Infrastructure\Database\FullRollListWithCombinations.txt"));

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
