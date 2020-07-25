using Dice_game.Infrastructure;
using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
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
                @"C:\Coding\Dice-game\Dice-game\Dice-game\Dice-game\Infrastructure\Database\PatternProbabilities.txt");

            for (int i = 0; i < 18; i++)
            {
                var results = ComputePatternProbabilities(i, 10, 500, 500);
                patternProbabilitiesFile.WriteLine(results);
            }

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

            var patterns = new List<int[]>(18)
            {
                new int[] { 1 },
                new int[] { 1, 1 },
                new int[] { 1, 1, 1 },
                new int[] { 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1 },
                new int[] { 1, 1, 1, 1, 1, 1 },
                new int[] { 1, 2 },
                new int[] { 1, 2, 3 },
                new int[] { 1, 2, 3, 4 },
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 1, 1, 2 },
                new int[] { 1, 1, 1, 2 },
                new int[] { 1, 1, 2, 2 },
                new int[] { 1, 1, 2, 2, 3 },
                new int[] { 1, 1, 1, 2, 2 },
                new int[] { 1, 1, 1, 2, 2, 2 },
                new int[] { 1, 1, 2, 3 },
                new int[] { 1, 1, 2, 2, 3, 3 }
            };

            var probabilities = new List<decimal[]>(numberOfRolls);
            var random = new Random();

            var diceToGet = patterns[index];
            var c = new Combination(CombinationType.Ones)
            {
                Dice = diceToGet
            };

            for (int i = 0; i < numberOfRolls; i++)
            {
                probabilities.Add(new decimal[numberOfRounds]);
                for (int r = 0; r < numberOfRounds; r++)
                {
                    probabilities[i][r] = 0;
                    for (int j = 0; j < numberOfIterations; j++)
                    {
                        var roll = i + 1;
                        var matchedDiceNumber = 0;
                        c.Dice = diceToGet;

                        while (roll > 0)
                        {
                            roll--;
                            int[] currentDice = Enumerable.Repeat(0, 6 - matchedDiceNumber)
                                                    .Select(i => random.Next(1, 7))
                                                    .ToArray();
                            c.Dice = c.MissingDiceToCompleteCombination(currentDice);
                            if (c.Dice.Length == 0)
                            {
                                probabilities[i][r]++;
                                break;
                            }

                            matchedDiceNumber = diceToGet.Length - c.Dice.Length;
                        }
                    }

                    probabilities[i][r] = probabilities[i][r] / numberOfIterations;
                }
                Console.WriteLine($"Roll #: {i + 1} completed");
            }

            var result = new StringBuilder();
            result.Append(GameUtility.CreateGenericPatternFromDice(diceToGet));
            result.Append(" ");
            result.Append(string.Join(' ', probabilities.Select(x => Math.Round(x.Average(), 5))));

            return result.ToString();
        }
    }
}
