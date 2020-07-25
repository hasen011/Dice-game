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
                            "AssignDice 666666",
                            "AssignCombination 0",
                            "AssignDice 666666",
                            "AssignCombination 0",
                            "AssignDice 666666",
                            "AssignCombination 0",
                            "AssignDice 111666",
                            "AssignCombination 0",
                            "AssignDice 222666",
                            "AssignCombination 0",
                            "AssignDice 333666",
                            "AssignCombination 0",
                            "AssignDice 444666",
                            "AssignCombination 0",
                            "AssignDice 555666",
                            "AssignCombination 0",
                            "AssignDice 111666",
                            "AssignCombination 0",
                            "AssignDice 343646",
                            "AssignCombination 0",
                            "99 123456",
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
