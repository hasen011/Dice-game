using Dice_game.Infrastructure.Utility;
using Dice_game.PlayerDomain;
using Dice_game.PlayerDomain.Utility;
using Dice_game.Infrastructure;

namespace Dice_game
{
    class Program
    {
        static void Main(string[] args)
        {
            var readInput = new ActionReader();
            var player1 = new Player(PlayerType.Human, "test_player_1");
            var player2 = new Player(PlayerType.Human, "test_player_2");

            var players = new[] { player1, player2 };

            var game = new Game(players, Round.Two);
            game.StartGame();
        }
    }
}
