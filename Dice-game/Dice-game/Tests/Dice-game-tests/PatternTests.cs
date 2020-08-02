using Dice_game.Infrastructure;
using Dice_game.Infrastructure.Utility;
using System;
using System.Linq;
using Xunit;

namespace Dice_game_tests
{
    public class PatternTests
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
            foreach (var rollWithCombinations in combinationList.ReadFullRollListWithAllMatchingCombinations())
            {
                dice = rollWithCombinations.Key;
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

        [Fact]
        public void GenericPatterns_CheckIfWeHaveAllPatternProbabilities()
        {
            var patternProbabilities = GameUtility.GetPatternProbabilities();
            var combinationList = new CombinationList();
            var isPatternMissing = false;

            // Use the list of all possible rolls to test if we have all pattern probabilities needed
            foreach (var rollWithCombinations in combinationList.ReadFullRollListWithAllMatchingCombinations())
            {
                var rolledDice = rollWithCombinations.Key;

                foreach (var c in combinationList.Combinations)
                {
                    var precomputedProbabilitiesCount = patternProbabilities.First().Value.Length;
                    c.SetProbabilitiesAndEVToCompleteCombinationWithinAllAvailableRolls(precomputedProbabilitiesCount, rolledDice, patternProbabilities);

                    if (c.ProbabilitiesToCompleteCombinationWithinAllAvailableRolls == null || c.ProbabilitiesToCompleteCombinationWithinAllAvailableRolls.Length != precomputedProbabilitiesCount)
                    {
                        isPatternMissing = true;
                        break;
                    }

                    if (c.ExpectedValuesForCombinationWithinAllAvailableRolls == null || c.ExpectedValuesForCombinationWithinAllAvailableRolls.Length != precomputedProbabilitiesCount)
                    {
                        isPatternMissing = true;
                        break;
                    }
                }

                if (isPatternMissing)
                {
                    break;
                }
            }

            Assert.False(isPatternMissing);
        }


    }
}
