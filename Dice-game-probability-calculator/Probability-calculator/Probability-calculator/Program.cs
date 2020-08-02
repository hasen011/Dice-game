using Dice_game.Infrastructure;
using Dice_game.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Probability_calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            using StreamWriter patternProbabilitiesFile = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\PatternProbabilities.txt");

            for (int i = 0; i < 35; i++)
            {
                var results = ComputePatternProbabilities(i, 5, 100, 100);
                patternProbabilitiesFile.WriteLine(results);
            }


            /*var charA = new char[] { '1', '2', '3', '4', '5', '6' };
            int d = 6;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (var n = 0; n < 50000000; n++)
            {
                var key = new string(charA) + d.ToString();
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);

            watch = System.Diagnostics.Stopwatch.StartNew();
            for (var n = 0; n < 50000000; n++)
            {
                //var key = new string(charA) + d.ToString();
                var key = new StringBuilder(7).Append(charA).Append(d.ToString()).ToString();
            }
            watch.Stop();
            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);*/

            //var key = new StringBuilder(7).Append(patternOfMissingDice).Append(rollingDiceCount.ToString()).ToString();

            /*var pattern = new[] { 1, 2, 3, 4, 5, 6 };
            var currentDice = new[] { 4, 2, 2, 4, 2, 4 };
            var N = 1000000;
            var random = new Random();

            var combinationList = new CombinationList();
            combinationList.CreateFullListOfAllPossibleRollsWithPossibleCombinations();

            var combination = new Combination(CombinationType.Threes)
            {
                Dice = new int[] { 3, 3, 3 }
            };

            var x = combinationList.LookupMatchingCombinations(new int[] { 3, 3, 3, 3, 4, 4 });

            var ppp = new PatternList();
            ppp.CreatePatternProbabilities(1);

            var p = new Player(PlayerType.Human)
            {
                TotalNumberOfRolls = 10000
            };

            var patterns = GameUtility.GetPatterns();
            while (p.TotalNumberOfRolls > 0)
            {
                p.RollDice();
                
                foreach (var c in combinationList.Combinations)
                {
                    var m = c.MissingDiceToCompleteCombination(p.CurrentDice);

                    var pat = GameUtility.CreateGenericPatternFromDice(m);

                    if (!patterns.Contains(pat))
                    {
                        Console.WriteLine(string.Join(',', pat));
                    }                   
                }
            }

            var xx = GameUtility.CreateGenericPatternFromDice(new int[] { 4, 1, 4, 4, 1 });
            var yy = combination.MissingDiceToCompleteCombination(new int[] { 2, 1, 3, 3, 5, 1 });

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (var n = 0; n < N; n++)
            {
                for (var i = 0; i < currentDice.Length; i++)
                {
                    currentDice[i] = random.Next(1, 7);
                }
                combinationList.LookupMatchingCombinations(currentDice);
            }
            foreach (var x in combinationList.LookupCombination(currentDice))
            {
                Console.WriteLine(x.CombinationType);
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);


            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);*/
        }

        /// <summary>
        /// Manual method to compute pattern probablity.
        /// </summary>
        /// <param name="index"></param>
        static string ComputePatternProbabilities(int index, int numberOfRolls, int numberOfRounds, int numberOfIterations)
        {
            // TODO: Add comments

            // These represent all possible patterns of missing to dice to finish any combination. Note that it doesn't matter
            // what dice values we choose to represent the patterns.
            // Also, there is a special case for combinations ones through sixes where any of these combinations can be completed
            // using three to all six dice. This means we need to compute probabilities, i.e. get value of 6 with 4 remaining dice
            // Format of the dictionary: <remainingDice, pattern>
            var patterns = new List<(int rollingDiceCount, int[] pattern)>(36)
            {
                ( 0, new int[] { } ),
                ( 1, new int[] { } ),
                ( 2, new int[] { } ),
                ( 3, new int[] { } ),

                ( 1, new int[] { 1 } ),
                ( 2, new int[] { 1 } ),
                ( 3, new int[] { 1 } ),
                ( 4, new int[] { 1 } ),

                ( 2, new int[] { 1, 1 } ),
                ( 3, new int[] { 1, 1 } ),
                ( 4, new int[] { 1, 1 } ),
                ( 5, new int[] { 1, 1 } ),

                ( 3, new int[] { 1, 1, 1 } ),
                ( 4, new int[] { 1, 1, 1 } ),
                ( 5, new int[] { 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1 } ),

                ( 4, new int[] { 1, 1, 1, 1 } ),
                ( 5, new int[] { 1, 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1, 1 } ),

                ( 5, new int[] { 1, 1, 1, 1, 1 } ),
                ( 6, new int[] { 1, 1, 1, 1, 1 } ),

                ( 6, new int[] { 1, 1, 1, 1, 1, 1 } ),

                // Any of the following ones can only complete a combination which consists of 6 dice
                ( 2, new int[] { 1, 2 } ),
                ( 3, new int[] { 1, 2, 3 } ),
                ( 4, new int[] { 1, 2, 3, 4 } ),
                ( 5, new int[] { 1, 2, 3, 4, 5 } ),
                ( 6, new int[] { 1, 2, 3, 4, 5, 6 } ),
                ( 3, new int[] { 1, 1, 2 } ),
                ( 4, new int[] { 1, 1, 1, 2 } ),
                ( 4, new int[] { 1, 1, 2, 2 } ),
                ( 5, new int[] { 1, 1, 2, 2, 3 } ),
                ( 5, new int[] { 1, 1, 1, 2, 2 } ),
                ( 6, new int[] { 1, 1, 1, 2, 2, 2 } ),
                ( 4, new int[] { 1, 1, 2, 3 } ),
                ( 6, new int[] { 1, 1, 2, 2, 3, 3 } )
            };

            var probabilities = new List<decimal[]>(numberOfRolls);
            var random = new Random();

            var rollingDiceCount = patterns[index].rollingDiceCount;
            var diceToGet = patterns[index].pattern;
            var c = new Combination(CombinationType.Ones)
            {
                Dice = diceToGet
            };

            // How many rolls a player has (we will probably cap it ~100)
            for (int i = 0; i < numberOfRolls; i++)
            {
                // For each # of available rolls
                probabilities.Add(new decimal[numberOfRounds]);

                // We run the expiriment 'numberOfIterations' times for 'numberOfRounds' times (average per round)
                // Basically - how many times we succeed in 'numberOfIterations' attempts
                // averageg over 'numberOfRounds'
                for (int r = 0; r < numberOfRounds; r++)
                {
                    probabilities[i][r] = 0;
                    for (int j = 0; j < numberOfIterations; j++)
                    {
                        // total number of rolls available
                        var roll = i + 1;
                        var matchedDiceNumber = 0;
                        c.Dice = diceToGet;

                        while (roll > 0)
                        {
                            roll--;
                            int[] currentDice = Enumerable.Repeat(0, rollingDiceCount - matchedDiceNumber)
                                                    .Select(i => random.Next(1, 7))
                                                    .ToArray();
                            // Check what dice is missing to complete the combination
                            c.Dice = c.MissingDiceToCompleteCombination(currentDice);

                            // If no dice is missing then add one to the list of completed runs
                            if (c.Dice.Length == 0)
                            {
                                probabilities[i][r]++;
                                break;
                            }

                            matchedDiceNumber = diceToGet.Length - c.Dice.Length;
                        }
                    }

                    // Compute how many we succeeded withing 'numberOfIterations' attempts
                    probabilities[i][r] = probabilities[i][r] / numberOfIterations;
                }
                Console.WriteLine($"Pattern: {index + 1} / {patterns.Count}, Roll: {i + 1} / {numberOfRolls} completed");
            }

            var result = new StringBuilder();
            result.Append(GameUtility.CreateGenericPatternFromDice(diceToGet));
            result.Append($" {rollingDiceCount} ");

            // Average the success rates
            result.Append(string.Join(' ', probabilities.Select(x => Math.Round(x.Average(), 6))));

            return result.ToString();
        }

        static void CreatePatternProbabilities(int numberOfRolls)
        {
            using StreamWriter file = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\Patterns.txt");

            for (var r = 0; r < numberOfRolls; r++)
            {


                Console.WriteLine(string.Join(",", GenerateDice(2)));
            }
        }


        /// <summary>
        /// Creates a txt file with all possible rolls we can get and all possible matching combinations.
        /// </summary>
        static void CreateFullListOfAllPossibleRollsWithPossibleCombinations()
        {
            var dice = new int[] { 0, 0, 0, 0, 0, 0 }.ToList();
            var rollWithPossibleCombinations = new StringBuilder(56);
            Console.WriteLine(Directory.GetCurrentDirectory());
            using StreamWriter file = new StreamWriter(
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\FullRollListWithCombinations.txt");

            // Load list of all possible combinations (dice patterns)
            var combinations = File.ReadAllLines(@"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Database\Combinations.txt");

            var combinationList = combinations.Select(c =>
            {
                var temp = c.Split(" ");
                var dicePattern = temp[0].ToCharArray().Select(x => x - '0').ToArray();
                Enum.TryParse(temp[1], true, out CombinationType combinationType);

                return new Combination(combinationType, dicePattern);
            }).ToArray();

            // Maps dice (combination) pattern to a combination object
            var combinationsLookup = combinationList.ToLookup(x => x.Dice, x => x, new ArrayEqualityComparer());

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
                                        if (combinationsLookup.Contains(z))
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


        static char[] GenerateDice(int len)
        {
            var random = new Random();

            var rolledDice = new char[len];

            for (var i = 0; i < rolledDice.Length; i++)
            {
                rolledDice[i] = (char)(random.Next(0, 6) + 'a');
            }

            return rolledDice;
        }


    }
}
