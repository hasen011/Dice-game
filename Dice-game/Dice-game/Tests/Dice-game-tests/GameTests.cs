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
        public void GameTest_SimpleRunToFinishRoundOne()
        {
            var game = new Game(new[] { new Player(PlayerType.Human, "test_player_1",
                    new ActionReaderSequence(
                        new [] { 
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign sixes 6,6,6
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign poker 6,6,6,6
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign general 6,6,6,6,6,6
                            "AssignDice 111666",
                            "AssignCombination 0", // Assign ones
                            "AssignDice 222666",
                            "AssignCombination 0", // Assign twos
                            "AssignDice 333666",
                            "AssignCombination 0", // Assign threes
                            "AssignDice 444666",
                            "AssignCombination 0", // Assign fours
                            "AssignDice 555666",
                            "AssignCombination 0", // Assign fives
                            "AssignDice 111666",
                            "AssignCombination 0", // Assign triples 1,1,1,6,6,6
                            "AssignDice 343646",
                            "AssignCombination 0", // Assign doubles 3,3,4,4,6,6
                            "99 123456", // AssignDice
                            "3 0", // AssignCombination, straight 1,2,3,4,5,6
                            "EndGame"})),
                new Player(PlayerType.Human, "test_player_2",
                    new ActionReaderSequence(new [] { "EndTurn", "5", "5", "5", "5", "5", "5", "5", "5", "5", "5" })) }); // Skip turn everytime

            Assert.False(game.EndGame);
            game.StartGame();

            Assert.True(game.EndGame);
            Assert.Equal(191 + 22, game.Players[0].Board.TotalScore);
            Assert.Equal(22, game.Players[1].Board.TotalScore);
            Assert.Equal(2, game.Players[0].TotalNumberOfRolls); // Games ends as Player takes the turn
            Assert.Equal(0, game.Players[1].TotalNumberOfRolls);
        }

        [Fact]
        public void GameTest_SimpleRunToFinishAllThreeRounds()
        {
            var game = new Game(new[] { new Player(PlayerType.Human, "test_player_1",
                    new ActionReaderSequence(
                        new [] {
                            // Round One
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign sixes 6,6,6
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign poker 6,6,6,6
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign general 6,6,6,6,6,6
                            "AssignDice 111666",
                            "AssignCombination 0", // Assign ones
                            "AssignDice 222666",
                            "AssignCombination 0", // Assign twos
                            "AssignDice 333666",
                            "AssignCombination 0", // Assign threes
                            "AssignDice 444666",
                            "AssignCombination 0", // Assign fours
                            "AssignDice 555666",
                            "AssignCombination 0", // Assign fives
                            "AssignDice 111666",
                            "AssignCombination 0", // Assign triples 1,1,1,6,6,6
                            "AssignDice 343646",
                            "AssignCombination 0", // Assign doubles 3,3,4,4,6,6
                            "AssignDice 123456",
                            "AssignCombination 0", // Assign straight 1,2,3,4,5,6

                            // Round Two
                            "6", // Pick sixes to play
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign sixes 6,6,6
                            "10", // Pick poker to play
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign poker 6,6,6,6
                            "11", // Pick general to play
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign general 6,6,6,6,6,6
                            "1", // Pick ones to play
                            "AssignDice 111666",
                            "AssignCombination 0", // Assign ones
                            "2", // Pick twos to play
                            "AssignDice 222666",
                            "AssignCombination 0", // Assign twos
                            "3", // Pick threes to play
                            "AssignDice 333666",
                            "AssignCombination 0", // Assign threes
                            "4", // Pick fours to play
                            "AssignDice 444666",
                            "AssignCombination 0", // Assign fours
                            "5", // Pick fives to play
                            "AssignDice 555666",
                            "AssignCombination 0", // Assign fives
                            "8", // Pick triples to play
                            "AssignDice 111666",
                            "AssignCombination 0", // Assign triples 1,1,1,6,6,6
                            "7", // Pick doubles to play
                            "AssignDice 343646",
                            "AssignCombination 0", // Assign doubles 3,3,4,4,6,6
                            "9", // Pick straight to play
                            "AssignDice 123456",
                            "AssignCombination 0", // Assign straight 1,2,3,4,5,6

                            // Round Three
                            
                            "AssignDice 111666",
                            "AssignCombination 0", // Assign ones
                            "AssignDice 222666",
                            "AssignCombination 0", // Assign twos
                            "AssignDice 333666",
                            "AssignCombination 0", // Assign threes
                            "AssignDice 444666",
                            "AssignCombination 0", // Assign fours
                            "AssignDice 555666",
                            "AssignCombination 0", // Assign fives
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign sixes 6,6,6
                            "AssignDice 343646",
                            "AssignCombination 0", // Assign doubles 3,3,4,4,6,6
                            "AssignDice 111666",
                            "AssignCombination 0", // Assign triples 1,1,1,6,6,6
                            "AssignDice 123456",
                            "AssignCombination 0", // Assign straight 1,2,3,4,5,6
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign poker 6,6,6,6
                            "AssignDice 666666",
                            "AssignCombination 0", // Assign general 6,6,6,6,6,6
                            // Game should end here
                              })),
                new Player(PlayerType.Human, "test_player_2",
                    new ActionReaderSequence(new [] { "5", "5", "5", "5", "5", "5", "5", "5", "5", "5", "5", // Round One
                                                      "1", "5",
                                                      "2", "5",
                                                      "3", "5",
                                                      "4", "5",
                                                      "5", "5",
                                                      "6","5",
                                                      "7", "5",
                                                      "8", "5",
                                                      "9", "5",
                                                      "10", "5",
                                                      "11", "5", // Round Two
                                                      "5", "5", "5", "5", "5", "5", "5", "5", "5", "5", "5", // Round Three
                                                    })) }); // Skip turn everytime

            Assert.False(game.EndGame);
            game.StartGame();

            Assert.True(game.EndGame);
            Assert.Equal(191*3 + 22*3 , game.Players[0].Board.TotalScore);
            Assert.Equal(66, game.Players[1].Board.TotalScore);
            Assert.Equal(0, game.Players[0].TotalNumberOfRolls);
            Assert.Equal(0, game.Players[1].TotalNumberOfRolls);
        }

        [Fact]
        public void GameTest_TestEndGameActionInTheSecondRound()
        {
            var game = new Game(new[] { new Player(PlayerType.Human, "test_player_1",
                    new ActionReaderSequence(new [] { "1", "EndGame" })) }, Round.Two); // End Game

            Assert.False(game.EndGame);
            game.StartGame();
            Assert.Equal(CombinationType.Ones, game.Players[0].CombinationToPlay);
            Assert.True(game.EndGame);
        }



    }
}
