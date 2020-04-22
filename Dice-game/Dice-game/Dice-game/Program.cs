using Dice_game.Infrastructure;
using Dice_game.PlayerDomain;
using System;
using System.Collections.Generic;

namespace Dice_game
{
    class Program
    {
        static void Main(string[] args)
        {
            var readInput = new ReadInput();
            var player1 = new Player(PlayerType.Human, readInput);
            var player2 = new Player(PlayerType.Human, readInput);

            var players = new[] { player1, player2 };

            var game = new Game(players);
            game.StartGame();
        }
    }
}
