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
                    new ActionReaderSequence(
                        new [] { 
                            "AssignDice 6,6,6,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 6,6,6,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 6,6,6,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 1,1,1,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 2,2,2,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 3,3,3,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 4,4,4,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 5,5,5,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 1,1,1,6,6,6",
                            "AssignCombination 0",
                            "AssignDice 3,4,3,6,4,6",
                            "AssignCombination 0",
                            "6 1,2,3,4,5,6",
                            "3 0" })),
                new Player(PlayerType.Human,
                    new ActionReaderSequence(new [] { "Yield", "5", "5", "5", "5", "5", "5", "5", "5", "5", "5" })) });

            Assert.False(game.LastRound);
            game.StartGame();

            Assert.True(game.LastRound);
            Assert.Equal(191, game.Players[0].Board.TotalScore);
            Assert.Equal(0, game.Players[1].Board.TotalScore);
            Assert.Equal(24, game.Players[0].TotalNumberOfRolls);
        }





    }
}
