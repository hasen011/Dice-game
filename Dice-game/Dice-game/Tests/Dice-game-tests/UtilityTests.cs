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
            var patterns = GameUtility.GetPatterns();
            // Add empty pattern for simplicity of this test
            patterns.Add(new char[] { });
            var patternFrequencies = patterns.ToDictionary(x => x, x => 0, new ArrayEqualityComparerChar());
            var combinationList = new CombinationList();
            var isPatternMissing = false;

            var count = 0;
            foreach (var roll in combinationList.ReadFullRollListWithAllMatchingCombinations())
            {
                dice = roll.Key;
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
