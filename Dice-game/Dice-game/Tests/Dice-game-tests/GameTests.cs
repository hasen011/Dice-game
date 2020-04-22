using Dice_game.Infrastructure;
using Dice_game.PlayerDomain;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Dice_game_tests
{
    public class GameTests
    {
        private readonly ITestOutputHelper _output;

        public GameTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void GameTest_SimpleRunToFinish()
        {
            var game = new Game(new[] { new Player(PlayerType.Human,
                    new ReadInputTest(
                        new [] { "3", "0", "3", "0", "3", "0", "3", "0", "3", "0", "3", "0", "3", "0", "3", "0", "3", "0", "3", "0", "3", "0" })),
                new Player(PlayerType.Human,
                    new ReadInputTest(new [] { "5", "5", "5", "5", "5", "5", "5", "5", "5", "5", "5" })) });
            game.StartGame();

            // Player 1 is iassigning combinations, Player 2 is always yielding.
            // Player 1 - Assign 6s
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 6, 6, 6, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign Poker
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 6, 6, 6, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign General
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 6, 6, 6, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign 1s
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 1, 1, 1, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign 2s
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 2, 2, 2, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign 3s
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 3, 3, 3, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign 4s
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 4, 4, 4, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign 1s
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 5, 5, 5, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign Triples
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 1, 1, 1, 6, 6, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign Pairs
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 3, 4, 3, 6, 4, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");
            // Player 2 - Yield
            _output.WriteLine("5");

            // Player 1 - Assign Straight
            AssignDice(game.Players[game.PlayerToPlay].CurrentDice, new[] { 1, 2, 3, 4, 5, 6 });
            _output.WriteLine("3");
            _output.WriteLine("0");

            Assert.True(game.LastRound);
            // Player 2 - Yield
            _output.WriteLine("5");
        }



        private void AssignDice(int[] currentDice, int[] diceToAssign)
        {
            for (var i = 0; i < currentDice.Length; i++)
            {
                currentDice[0] = diceToAssign[i];
            }
        }
    }


    class ReadInputTest : ReadInput
    {
        private int index = 0;
        private string[] InputSequence { get; set; }

        public ReadInputTest(string[] inputSequence)
        {
            InputSequence = inputSequence;
        }
        public override string GetNextInput()
        {
            return InputSequence[index++];
        }
    }
}
