using Dice_game.Infrastructure;
using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
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
            var N = 1000000;
            var random = new Random();

            var combinationList = new CombinationList();

            var combination = new Combination(CombinationType.Threes)
            {
                Dice = new int[] { 3, 3, 3 }
            };

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
            /*foreach (var x in combinationList.LookupCombination(currentDice))
            {
                Console.WriteLine(x.CombinationType);
            }*/
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);


            elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine(elapsedMs);
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
