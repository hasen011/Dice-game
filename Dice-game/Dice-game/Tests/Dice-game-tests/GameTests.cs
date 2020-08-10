using Dice_game.Infrastructure;
using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
using Dice_game.PlayerDomain.Utility;
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
            var game = new Game(new[] { new Player(PlayerType.Human, "test_player_1",
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
                            "3 0",
                            "EndGame"})),
                new Player(PlayerType.Human, "test_player_2",
                    new ActionReaderSequence(new [] { "EndTurn", "5", "5", "5", "5", "5", "5", "5", "5", "5", "5" })) });

            Assert.False(game.EndGame);
            game.StartGame();

            Assert.True(game.EndGame);
            Assert.Equal(191, game.Players[0].Board.TotalScore);
            Assert.Equal(0, game.Players[1].Board.TotalScore);
            Assert.Equal(24, game.Players[0].TotalNumberOfRolls);
        }



    }
}
