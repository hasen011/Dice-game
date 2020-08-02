using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
using Dice_game.PlayerDomain.Utility;
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
                RolledDice = new[] { 2, 2, 2, 2, 2, 2 },
                FixedDice = new[] { true, true, true, true, true, true },
                TotalNumberOfRolls = 10
            };
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Equal(8, p.CurrentPossibleCombinations.Length);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Twos && x.Dice.SequenceEqual(new[] { 2, 2, 2 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Twos && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Twos && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Twos && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2, 2 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.General && x.Dice.SequenceEqual(new[] { 2, 2, 2, 2, 2, 2 }));

            // Change current dice
            p.RolledDice[0] = 4;
            p.RolledDice[1] = 4;
            p.RolledDice[2] = 4;
            p.RolledDice[3] = 6;
            p.RolledDice[4] = 6;
            p.RolledDice[5] = 6;
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Equal(3, p.CurrentPossibleCombinations.Length);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Sixes && x.Dice.SequenceEqual(new[] { 6, 6, 6 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Triples && x.Dice.SequenceEqual(new[] { 4, 4, 4, 6, 6, 6 }));

            // Change current dice
            p.RolledDice[0] = 6;
            p.RolledDice[1] = 4;
            p.RolledDice[2] = 5;
            p.RolledDice[3] = 1;
            p.RolledDice[4] = 3;
            p.RolledDice[5] = 2;
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Single(p.CurrentPossibleCombinations);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Straight && x.Dice.SequenceEqual(new[] { 1, 2, 3, 4, 5, 6 }));

            // Change current dice
            p.RolledDice[0] = 6;
            p.RolledDice[1] = 4;
            p.RolledDice[2] = 5;
            p.RolledDice[3] = 6;
            p.RolledDice[4] = 5;
            p.RolledDice[5] = 4;
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Single(p.CurrentPossibleCombinations);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Pairs && x.Dice.SequenceEqual(new[] { 4, 4, 5, 5, 6, 6 }));

            // Change current dice
            p.RolledDice[0] = 6;
            p.RolledDice[1] = 4;
            p.RolledDice[2] = 5;
            p.RolledDice[3] = 6;
            p.RolledDice[4] = 1;
            p.RolledDice[5] = 2;
            p.RollDice(); // This should not change any dice but it will find matching combinations

            Assert.Empty(p.CurrentPossibleCombinations);
        }

        [Fact]
        public void CombinationLookup_ReturnsCorrectlyAfterRerolls()
        {
            var p = new Player(PlayerType.Human)
            {
                RolledDice = new[] { 4, 1, 4, 6, 4, 4 },
                TotalNumberOfRolls = 10
            };

            _output.WriteLine($"[4, 1, 4, 6, 4, 4]: {string.Join(", ", p.RolledDice)}");

            // Fix all dice and re-roll. This is done just so we get the matching combinations without changing any die
            p.FixDice(new[] { 0, 1, 2, 3, 4, 5 });
            p.RollDice();

            _output.WriteLine($"[4, 1, 4, 6, 4, 4]: {string.Join(", ", p.RolledDice)}");

            Assert.Equal(3, p.CurrentPossibleCombinations.Length);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));

            p.FixDice(new[] { 0, 2, 4, 5 });
            p.RollDice();

            // Make sure we didn't create new combinations by re-rolling
            while (p.RolledDice[1] == 4 || p.RolledDice[3] == 4)
            {
                p.RollDice();
            }

            // We should still have the same matching combinations
            Assert.Equal(3, p.CurrentPossibleCombinations.Length);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));

            _output.WriteLine($"[4, _, 4, _, 4, 4]: {string.Join(", ", p.RolledDice)}");

            p.RollDice(); // Roll for fun

            // Change current dice
            p.RolledDice[0] = 1;
            p.RolledDice[1] = 5;
            p.RolledDice[2] = 1;
            p.RolledDice[3] = 6;
            p.RolledDice[4] = 6;
            p.RolledDice[5] = 5;

            // Fix all dice and roll
            p.FixDice(new[] { 0, 1, 2, 3, 4, 5 });
            p.RollDice();

            _output.WriteLine($"[1, 5, 1, 6, 6, 5]: {string.Join(", ", p.RolledDice)}");

            Assert.Single(p.CurrentPossibleCombinations);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Pairs && x.Dice.SequenceEqual(new[] { 1, 1, 5, 5, 6, 6 }));
        }

        [Fact]
        public void CombinationLookup_CompletedCombinationsAreNotReturned()
        {
            var p = new Player(PlayerType.Human)
            {
                RolledDice = new[] { 4, 1, 4, 6, 4, 4 },
                FixedDice = new[] { true, true, true, true, true, true },
                TotalNumberOfRolls = 10
            };

            // Roll dice to get matching combinations
            p.RollDice();

            _output.WriteLine($"[4, 1, 4, 6, 4, 4]: {string.Join(", ", p.RolledDice)}");

            Assert.Equal(3, p.CurrentPossibleCombinations.Length);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Fours && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));

            p.TryAssignCombination(1);          

            // Fix all dice and re-roll. This is done just so we get the matching combinations without changing any die
            p.FixDice(new[] { 0, 1, 2, 3, 4, 5 });
            p.RollDice();

            _output.WriteLine($"[4, 1, 4, 6, 4, 4]: {string.Join(", ", p.RolledDice)}");

            Assert.Single(p.CurrentPossibleCombinations);
            Assert.Contains(p.CurrentPossibleCombinations,
                x => x.CombinationType == CombinationType.Poker && x.Dice.SequenceEqual(new[] { 4, 4, 4, 4 }));
        }



    }
}
