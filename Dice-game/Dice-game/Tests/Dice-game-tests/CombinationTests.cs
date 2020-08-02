using Dice_game.Infrastructure;
using Dice_game.Infrastructure.Utility;
using Xunit;
using Xunit.Abstractions;


namespace Dice_game_tests
{
    public class CombinationTests
    {
        private readonly ITestOutputHelper _output;

        public CombinationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CombinationTest_CreatesGenericPatternCorrectly()
        {
            Assert.Equal(new char[] { 'a' }, GameUtility.CreateGenericPatternFromDice(new int[] { 1 }));
            Assert.Equal(new char[] { 'a', 'b' }, GameUtility.CreateGenericPatternFromDice(new int[] { 3, 1 }));
            Assert.Equal(new char[] { 'a', 'a', 'b' }, GameUtility.CreateGenericPatternFromDice(new int[] { 3, 1, 3 }));
            Assert.Equal(new char[] { 'a', 'b', 'c' }, GameUtility.CreateGenericPatternFromDice(new int[] { 3, 1, 2 }));
            Assert.Equal(new char[] { 'a', 'a', 'b', 'b' }, GameUtility.CreateGenericPatternFromDice(new int[] { 1, 2, 1, 2 }));
            Assert.Equal(new char[] { 'a', 'a', 'a', 'b', 'c' }, GameUtility.CreateGenericPatternFromDice(new int[] { 5, 1, 2, 5, 5 }));
            Assert.Equal(new char[] { 'a', 'a', 'b', 'c', 'd', 'e' }, GameUtility.CreateGenericPatternFromDice(new int[] { 1, 2, 1, 3, 4, 5 }));
            Assert.Equal(new char[] { 'a', 'a', 'a', 'b', 'b', 'b' }, GameUtility.CreateGenericPatternFromDice(new int[] { 6, 5, 5, 6, 5, 6 }));
            Assert.Equal(new char[] { 'a', 'b', 'c', 'd', 'e', 'f' }, GameUtility.CreateGenericPatternFromDice(new int[] { 1, 2, 3, 4, 5, 6 }));
        }

        [Fact]
        public void CombinationTest_FindMissingDiceFromCombinationCorrectly()
        {
            var combination = new Combination(CombinationType.Threes)
            {
                Dice = new int[] { 3, 3, 3, 3 }
            };

            Assert.Equal(new int[] { 3, 3 }, combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 5, 5, 2, 3 }));
            Assert.Equal(new int[] { 3, 3, 3 }, combination.MissingDiceToCompleteCombination(new int[] { 6, 1, 5, 5, 2, 3 }));
            Assert.Equal(new int[] { 3 }, combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 3, 5, 2, 3 }));
            Assert.Equal(new int[] { }, combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 3, 3, 2, 3 }));

            combination = new Combination(CombinationType.Pairs)
            {
                Dice = new int[] { 1, 1, 3, 3, 5, 5 }
            };

            Assert.Equal(new int[] { 1, 1, 3, 3, 5, 5 }, combination.MissingDiceToCompleteCombination(new int[] { 6, 6, 4, 4, 2, 2 }));
            Assert.Equal(new int[] { 1, 3 }, combination.MissingDiceToCompleteCombination(new int[] { 6, 1, 5, 5, 2, 3 }));
            Assert.Equal(new int[] { 1, 5 }, combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 3, 5, 2, 3 }));
            Assert.Equal(new int[] { 1, 5, 5 }, combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 3, 3, 2, 3 }));
            Assert.Equal(new int[] { }, combination.MissingDiceToCompleteCombination(new int[] { 5, 1, 3, 3, 5, 1 }));
            Assert.Equal(new int[] { 1, 3, 5 }, combination.MissingDiceToCompleteCombination(new int[] { 5, 3, 1, 4, 2, 6 }));
        }

        [Fact]
        public void CombinationTest_CreateGenericPatternsForMissingDiceFromCombinationCorrectly()
        {
            var combination = new Combination(CombinationType.Threes)
            {
                Dice = new int[] { 3, 3, 3, 3 }
            };

            var missingDice = combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 5, 5, 2, 3 });
            Assert.Equal(new int[] { 3, 3 }, missingDice);
            var pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { 'a', 'a' }, pattern);

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 6, 1, 5, 5, 2, 3 });
            Assert.Equal(new int[] { 3, 3, 3 }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { 'a', 'a', 'a' }, pattern);

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 3, 5, 2, 3 });
            Assert.Equal(new int[] { 3 }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { 'a' }, pattern);

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 3, 3, 2, 3 });
            Assert.Equal(new int[] { }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { }, pattern);

            combination = new Combination(CombinationType.Pairs)
            {
                Dice = new int[] { 1, 1, 3, 3, 5, 5 }
            };

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 6, 6, 4, 4, 2, 2 });
            Assert.Equal(new int[] { 1, 1, 3, 3, 5, 5 }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { 'a', 'a', 'b', 'b', 'c', 'c' }, pattern);

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 6, 1, 5, 5, 2, 3 });
            Assert.Equal(new int[] { 1, 3 }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { 'a', 'b' }, pattern);

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 3, 5, 2, 3 });
            Assert.Equal(new int[] { 1, 5 }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { 'a', 'b' }, pattern);

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 3, 1, 3, 3, 2, 3 });
            Assert.Equal(new int[] { 1, 5, 5 }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { 'a', 'a', 'b' }, pattern);

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 5, 1, 3, 3, 5, 1 });
            Assert.Equal(new int[] { }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { }, pattern);

            missingDice = combination.MissingDiceToCompleteCombination(new int[] { 5, 3, 1, 4, 2, 6 });
            Assert.Equal(new int[] { 1, 3, 5 }, missingDice);
            pattern = GameUtility.CreateGenericPatternFromDice(missingDice);
            Assert.Equal(new char[] { 'a', 'b', 'c' }, pattern);
        }


    }
}