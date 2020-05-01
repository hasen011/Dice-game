using Dice_game.Infrastructure;
using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Dice_game_tests
{
    public class UtilityTests
    {
        [Fact]
        public void GenericPatterns_CheckIfWeHaveAllNeededPatterns()
        {
            var dice = new int[6] { 0, 0, 0, 0, 0, 0 };
            var patternFrequencies = GameUtility.GetPatterns().ToDictionary(x => x, x => 0, new ArrayEqualityComparerChar());
            var combinationList = new CombinationList();
            var isPatternMissing = false;

            var count = 0;
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
                                    count++;

                                    foreach (var com in combinationList.Combinations)
                                    {
                                        var missingDice = com.MissingDiceToCompleteCombination(dice);

                                        var pattern = GameUtility.CreateGenericPatternFromDice(missingDice);

                                        if (!patternFrequencies.ContainsKey(pattern))
                                        {
                                            isPatternMissing = true;
                                        }
                                        else
                                        {
                                            patternFrequencies[pattern]++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            Assert.False(isPatternMissing);
            Assert.Equal(Math.Pow(6, 6) * combinationList.Combinations.Length, patternFrequencies.Sum(x => x.Value));
            Assert.Equal(Math.Pow(6, 6), count);
            foreach (var pat in patternFrequencies)
            {
                Assert.True(pat.Value > 0);
            }
            
        }




    }
}
