using Dice_game.PlayerDomain;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Dice_game_tests
{
    public class PlayerTests
    {
        private readonly ITestOutputHelper _output;

        public PlayerTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void CombinationLookup_ReturnsCorrectly()
        {
            var p = new Player(PlayerType.Human)
            {
                // Fix all dice
                CurrentDice = new[] { 2, 2, 2, 2, 2, 2 },
                FixedDice = new[] { true, true, true, true, true, true }
            };
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Equal(8, p.CurrentMatchingCombinations.Length);
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Twos && x.Dice.SequenceEqual(new[] { 2, 2, 2 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Twos && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Twos && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Twos && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.General && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2, 2 }));

            // Change current dice
            p.CurrentDice[0] = 4;
            p.CurrentDice[1] = 4;
            p.CurrentDice[2] = 4;
            p.CurrentDice[3] = 6;
            p.CurrentDice[4] = 6;
            p.CurrentDice[5] = 6;
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Equal(3, p.CurrentMatchingCombinations.Length);
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Sixes && x.Dice.SequenceEqual(new[] { 6, 6, 6 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Triples && x.Dice.SequenceEqual(new[] { 4, 4, 4, 6, 6, 6 }));

            // Change current dice
            p.CurrentDice[0] = 6;
            p.CurrentDice[1] = 4;
            p.CurrentDice[2] = 5;
            p.CurrentDice[3] = 1;
            p.CurrentDice[4] = 3;
            p.CurrentDice[5] = 2;
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Single(p.CurrentMatchingCombinations);
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Straight && x.Dice.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6 }));

            // Change current dice
            p.CurrentDice[0] = 6;
            p.CurrentDice[1] = 4;
            p.CurrentDice[2] = 5;
            p.CurrentDice[3] = 6;
            p.CurrentDice[4] = 5;
            p.CurrentDice[5] = 4;
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Single(p.CurrentMatchingCombinations);
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Pairs && x.Dice.SequenceEqual(new[] { 4, 4, 5, 5, 6, 6 }));

            // Change current dice
            p.CurrentDice[0] = 6;
            p.CurrentDice[1] = 4;
            p.CurrentDice[2] = 5;
            p.CurrentDice[3] = 6;
            p.CurrentDice[4] = 1;
            p.CurrentDice[5] = 2;
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Empty(p.CurrentMatchingCombinations);
        }

        [Fact]
        public void CombinationLookup_ReturnsCorrectlyAfterRerolls()
        {
            var p = new Player(PlayerType.Human)
            {
                CurrentDice = new[] { 4, 1, 4, 6, 4, 4 }
            };

            _output.WriteLine($"[4, 1, 4, 6, 4, 4]: {string.Join(", ", p.CurrentDice)}");

            // Fix all dice and re-roll. This is done just so we get the matching combinations without changing any die
            p.FixDice("012345");
            p.RollDice();

            _output.WriteLine($"[4, 1, 4, 6, 4, 4]: {string.Join(", ", p.CurrentDice)}");

            Assert.Equal(3, p.CurrentMatchingCombinations.Length);
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));

            p.FixDice("0245");
            p.RollDice();

            // Make sure we didn't create new combinations by re-rolling
            while (p.CurrentDice[1] == 4 || p.CurrentDice[3] == 4)
            {
                p.RollDice();
            }

            // We should still have the same matching combinations
            Assert.Equal(3, p.CurrentMatchingCombinations.Length);
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));

            _output.WriteLine($"[4, _, 4, _, 4, 4]: {string.Join(", ", p.CurrentDice)}");

            p.RollDice(); // Roll for fun

            // Change current dice
            p.CurrentDice[0] = 1;
            p.CurrentDice[1] = 5;
            p.CurrentDice[2] = 1;
            p.CurrentDice[3] = 6;
            p.CurrentDice[4] = 6;
            p.CurrentDice[5] = 5;

            // Fix all dice and roll
            p.FixDice("012345");
            p.RollDice();

            _output.WriteLine($"[1, 5, 1, 6, 6, 5]: {string.Join(", ", p.CurrentDice)}");

            Assert.Single(p.CurrentMatchingCombinations);
            Assert.Contains(p.CurrentMatchingCombinations,
                x => x.CombinationType == CombinationType.Pairs && x.Dice.SequenceEqual(new[] { 1, 1, 5, 5, 6, 6 }));
        }
    }
}
